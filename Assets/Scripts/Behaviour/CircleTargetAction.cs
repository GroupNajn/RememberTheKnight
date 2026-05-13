using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using NUnit.Framework;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Circle Target", story: "[Self] circles around [Target] in a radius of [CircleRadius] with avoidance priotity [Priority] for [Duration] seconds", category: "Action", id: "4208390532c834c3ccafbfbd56ecfb3c")]
public partial class CircleTargetAction : Action
{

    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> CircleRadius;

    [SerializeReference] public BlackboardVariable<int> Priority;
    [SerializeReference] public BlackboardVariable<float> Duration;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private CharacterController characterController;
    private bool isClockwise;
    private Vector3 lastTargetPos;

    private bool shouldCorrect = false;
    private bool isCorrecting = false;
    private Vector3 currentCirclePoint;

    private float elapsedSeconds;
    protected override Status OnStart()
    {
        navMeshAgent = Self.Value;
        animator = Self.Value.gameObject.GetComponent<Animator>();
        characterController = Self.Value.gameObject.GetComponent<CharacterController>();

        lastTargetPos = Target.Value.position;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        elapsedSeconds = 0;
        navMeshAgent.velocity = Vector3.zero;
        currentCirclePoint = SampleCirclePoints(10);
        navMeshAgent.SetDestination(currentCirclePoint);
        navMeshAgent.avoidancePriority = Priority.Value;
        isClockwise = UnityEngine.Random.value > 0.5f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);
        if (elapsedSeconds >= Duration.Value)
        {
            if (navMeshAgent.hasPath)
            {
                Self.Value.transform.LookAt(navMeshAgent.steeringTarget);
                navMeshAgent.ResetPath();
            }
            return Status.Success;
        }
        elapsedSeconds += Time.deltaTime;

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
            navMeshAgent.avoidancePriority = Priority.Value;
            isCorrecting = false;
            navMeshAgent.ResetPath();
        }

        if (isCorrecting)
        {
            Vector3 targetPos = Target.Value.transform.position;
            navMeshAgent.avoidancePriority = Priority.Value + 10;
            bool shouldUpdateDestination =
                !Mathf.Approximately(lastTargetPos.x, targetPos.x) ||
                !Mathf.Approximately(lastTargetPos.y, targetPos.y) ||
                !Mathf.Approximately(lastTargetPos.z, targetPos.z);
            lastTargetPos = targetPos;
            if (shouldUpdateDestination)
            {
                currentCirclePoint = SampleCirclePoints(10);
                navMeshAgent.SetDestination(currentCirclePoint);
            }

            Vector3 worldDesiredVelocity = navMeshAgent.desiredVelocity / navMeshAgent.speed;
            worldDesiredVelocity *= 0.75f;
            Vector3 localDesiredVelocity = Self.Value.transform.InverseTransformDirection(worldDesiredVelocity);

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
            return Status.Running;
        }

        if (!isCorrecting && navMeshAgent.hasPath)
        {
            navMeshAgent.avoidancePriority = Priority.Value;
            navMeshAgent.ResetPath();
        }

        if (isClockwise)
        {
            if (NavMesh.SamplePosition(Self.Value.transform.position - Self.Value.transform.right, out NavMeshHit hit, 0.1f, navMeshAgent.areaMask))
                isClockwise = navMeshAgent.CalculatePath(hit.position, new NavMeshPath());
        }
        else
        {
            if (NavMesh.SamplePosition(Self.Value.transform.position + Self.Value.transform.right, out NavMeshHit hit, 0.1f, navMeshAgent.areaMask))
                isClockwise = !navMeshAgent.CalculatePath(hit.position, new NavMeshPath());
        }


        float sphereRadius = navMeshAgent.radius * 0.8f;
        float yOffset = navMeshAgent.height / 2;
        float maxDistance = navMeshAgent.radius + navMeshAgent.stoppingDistance;

        Vector3 origin = Self.Value.transform.position + Vector3.up * yOffset;

        RaycastHit[] result = isClockwise
            ? Physics.SphereCastAll(origin, sphereRadius, -Self.Value.transform.right, maxDistance)
            : Physics.SphereCastAll(origin, sphereRadius, Self.Value.transform.right, maxDistance);

        foreach (var hit in result)
        {
            if (Vector3.Distance(hit.point, Self.Value.transform.position) > Self.Value.stoppingDistance) break;
            if (!hit.collider.transform.IsChildOf(Self.Value.gameObject.transform) && hit.collider.gameObject != Self.Value.gameObject)
            {
                isClockwise = !isClockwise;
                break;
            }
        }
        if (isClockwise)
        {
            isClockwise =
                navMeshAgent.CalculatePath(Self.Value.transform.position - (Self.Value.transform.right * navMeshAgent.radius / 2), new NavMeshPath());
        }
        else
        {
            isClockwise =
                !navMeshAgent.CalculatePath(Self.Value.transform.position + (Self.Value.transform.right * navMeshAgent.radius / 2), new NavMeshPath());
        }

        float targetSpeedX = isClockwise ? -0.75f : 0.75f;
        animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, targetSpeedX, navMeshAgent.acceleration * Time.deltaTime));
        animator.SetFloat(YHash, Mathf.Lerp(currentSpeedZ, 0, navMeshAgent.acceleration * Time.deltaTime));

        return Status.Running;

    }

    protected override void OnEnd()
    {
        if (navMeshAgent.hasPath)
            navMeshAgent.ResetPath();

        navMeshAgent.updateRotation = true;
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

