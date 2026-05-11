using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using NUnit.Framework;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Circle Target", story: "[Self] circles around [Target] in a radius of [CircleRadius] for [Duration] seconds", category: "Action", id: "4208390532c834c3ccafbfbd56ecfb3c")]
public partial class CircleTargetAction : Action
{

    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> CircleRadius;
    [SerializeReference] public BlackboardVariable<float> Duration;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private CharacterController characterController;
    [SerializeReference] public BlackboardVariable<bool> IsClockwise;
    private Vector3 lastDestination;
    private float minMoveDistance;

    private bool shouldCorrect = false;
    private bool isCorrecting = false;
    private Vector3 currentCirclePoint;
    private float maxSpeed;

    private float elapsedSeconds;
    protected override Status OnStart()
    {
        navMeshAgent = Self.Value;
        animator = Self.Value.gameObject.GetComponent<Animator>();
        characterController = Self.Value.gameObject.GetComponent<CharacterController>();
        minMoveDistance = characterController != null ? characterController.minMoveDistance : 0.01f;

        lastDestination = Target.Value.position;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        elapsedSeconds = 0;
        navMeshAgent.velocity = Vector3.zero;
        currentCirclePoint = SampleCirclePoints(10);
        navMeshAgent.SetDestination(currentCirclePoint);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        elapsedSeconds += Time.deltaTime;
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);
        if (elapsedSeconds >= Duration.Value)
        {
            if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
            animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, 0, navMeshAgent.acceleration * Time.deltaTime));
            animator.SetFloat(YHash, Mathf.Lerp(currentSpeedZ, 0, navMeshAgent.acceleration * Time.deltaTime));
            if (Mathf.Approximately(currentSpeedX, 0) && Mathf.Approximately(currentSpeedZ, 0)) return Status.Success;
            else return Status.Running;
        }

        maxSpeed = Mathf.Clamp(Vector3.Distance(Self.Value.transform.position, Target.Value.position) / (1.2f * CircleRadius), 0.5f, 1f);
        navMeshAgent.nextPosition = Self.Value.transform.position;
        var dist = (Target.Value.position - Self.Value.transform.position).magnitude;
        shouldCorrect = dist > CircleRadius.Value + navMeshAgent.radius || dist < CircleRadius.Value - navMeshAgent.radius;

        Self.Value.transform.LookAt(Target.Value.position);
        if (shouldCorrect && !isCorrecting)
        {
            currentCirclePoint = SampleCirclePoints(10);
            isCorrecting = true;
        }

        if (isCorrecting && !shouldCorrect)
        {
            isCorrecting = false;
            navMeshAgent.ResetPath();
        }

        if (isCorrecting)
        {
            bool shouldUpdateDestination =
            !Mathf.Approximately(lastDestination.x, currentCirclePoint.x) ||
            !Mathf.Approximately(lastDestination.y, currentCirclePoint.y) ||
            !Mathf.Approximately(lastDestination.z, currentCirclePoint.z);
            lastDestination = currentCirclePoint;
            if (shouldUpdateDestination) navMeshAgent.SetDestination(currentCirclePoint);

            Vector3 worldDesiredVelocity = navMeshAgent.desiredVelocity / navMeshAgent.speed;
            Vector3 localDesiredVelocity = Self.Value.transform.InverseTransformDirection(worldDesiredVelocity) * maxSpeed;

            float desiredSpeedX = localDesiredVelocity.x;
            float desiredSpeedZ = localDesiredVelocity.z;

            animator.SetFloat(XHash, Mathf.Lerp(
                  currentSpeedX,
                  desiredSpeedX,
                  navMeshAgent.acceleration * Time.deltaTime
              ));
            animator.SetFloat(YHash, Mathf.Lerp(
                    currentSpeedZ,
                    desiredSpeedZ,
                    navMeshAgent.acceleration * Time.deltaTime
                ));
        }

        if (IsClockwise.Value)
        {
            if (NavMesh.SamplePosition(Self.Value.transform.position - Self.Value.transform.right, out NavMeshHit hit, 0.1f, navMeshAgent.areaMask))
                IsClockwise.Value = navMeshAgent.CalculatePath(hit.position, new NavMeshPath());
        }
        else
        {
            if (NavMesh.SamplePosition(Self.Value.transform.position + Self.Value.transform.right, out NavMeshHit hit, 0.1f, navMeshAgent.areaMask))
                IsClockwise.Value = !navMeshAgent.CalculatePath(hit.position, new NavMeshPath());
        }

        /* 
                float sphereRadius = navMeshAgent.radius * 0.8f;
                float yOffset = navMeshAgent.height / 2;

                Vector3 origin = Self.Value.transform.position + Vector3.up * yOffset;

                var result = IsClockwise.Value
                    ? Physics.SphereCastAll(origin, sphereRadius, -Self.Value.transform.right)
                    : Physics.SphereCastAll(origin, sphereRadius, Self.Value.transform.right);

                foreach (var hit in result)
                {
                    if (Vector3.Distance(hit.point, Self.Value.transform.position) > Self.Value.stoppingDistance) break;
                    if (!hit.collider.transform.IsChildOf(Self.Value.gameObject.transform) && hit.collider.gameObject != Self.Value.gameObject)
                    {
                        IsClockwise.Value = !IsClockwise.Value;
                        break;
                    }
                } */

        float targetSpeedX = IsClockwise.Value ? -maxSpeed : maxSpeed;
        animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, targetSpeedX, navMeshAgent.acceleration * Time.deltaTime));
        animator.SetFloat(YHash, Mathf.Lerp(currentSpeedZ, 0, navMeshAgent.acceleration * Time.deltaTime));

        return Status.Running;

    }

    protected override void OnEnd()
    {
        if (navMeshAgent.isOnNavMesh)
            navMeshAgent.ResetPath();

        animator.SetFloat(XHash, 0);
        animator.SetFloat(YHash, 0);
    }

    private Vector3 SampleCirclePoints(int sampleDensity)
    {
        Vector3 dir = navMeshAgent.transform.position - Target.Value.position;
        dir.y = 0;
        dir.Normalize();
        dir *= CircleRadius.Value;
        for (int i = 0; i < sampleDensity; i++)
        {

            if (NavMesh.SamplePosition(Quaternion.AngleAxis(sampleDensity / 360 * i, navMeshAgent.transform.up) * dir + Target.Value.position, out NavMeshHit hit, navMeshAgent.radius, navMeshAgent.areaMask))
            {
                return hit.position;
            }
        }
        return navMeshAgent.transform.position;
    }
}

