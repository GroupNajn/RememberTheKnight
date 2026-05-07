using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using NUnit.Framework;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Circle Target", story: "[Self] circles around [Target] in a radius of [CircleRadius]", category: "Action", id: "4208390532c834c3ccafbfbd56ecfb3c")]
public partial class CircleTargetAction : Action
{

    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> CircleRadius;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private CharacterController characterController;

    private Vector3 circlePoint;
    private bool isClockwise = true;
    private Vector3 lastDestination;
    private Vector3 lastTargetPos;
    private float minMoveDistance;

    private bool shouldCorrect = false;
    private Vector3 currentCirclePoint;
    private Queue<Vector3> circlePoints;

    protected override Status OnStart()
    {
        navMeshAgent = Self.Value;
        animator = Self.Value.gameObject.GetComponent<Animator>();
        characterController = Self.Value.gameObject.GetComponent<CharacterController>();
        minMoveDistance = characterController != null ? characterController.minMoveDistance : 0.01f;

        lastDestination = Target.Value.position;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.autoBraking = false;
        circlePoints = SampleCirclePoints(20);
        if (circlePoints.Count == 0) return Status.Failure;
        currentCirclePoint = circlePoints.Dequeue();
        navMeshAgent.SetDestination(currentCirclePoint);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 lookDir = Target.Value.position - Self.Value.transform.position;
        lookDir.y = 0;
        Self.Value.transform.rotation = Quaternion.LookRotation(lookDir);

        navMeshAgent.nextPosition = Self.Value.transform.position;

        var dist = (Target.Value.position - Self.Value.transform.position).magnitude;
        shouldCorrect = dist > CircleRadius.Value * 1.2 || dist < CircleRadius.Value * 0.8;
        if (shouldCorrect) circlePoints = SampleCirclePoints(5);
        if (circlePoints.Count == 0)
        {
            circlePoints = SampleCirclePoints(5);
            if (circlePoints.Count == 0) return Status.Failure;
        }

        if (!RegularNavigate(currentCirclePoint))
        {
            return Status.Running;
        }
        else
        {
            currentCirclePoint = circlePoints.Dequeue();
            return Status.Running;
        }

        return Status.Running;

        navMeshAgent.nextPosition = Self.Value.transform.position;
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);

        if (isClockwise)
        {
            animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, -1f, navMeshAgent.acceleration * Time.deltaTime));
            isClockwise = navMeshAgent.CalculatePath(Self.Value.transform.position - Self.Value.transform.right * (navMeshAgent.radius + navMeshAgent.stoppingDistance), new NavMeshPath());
        }
        else
        {
            animator.SetFloat(XHash, Mathf.Lerp(currentSpeedX, 1f, navMeshAgent.acceleration * Time.deltaTime));
            isClockwise = !navMeshAgent.CalculatePath(Self.Value.transform.position + Self.Value.transform.right * (navMeshAgent.radius + navMeshAgent.stoppingDistance), new NavMeshPath());
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        navMeshAgent.autoBraking = true;
    }

    private Queue<Vector3> SampleCirclePoints(int sampleDensity)
    {
        Vector3 dir = navMeshAgent.transform.position - Target.Value.position;
        dir.y = 0;
        dir.Normalize();
        dir *= CircleRadius.Value;
        Queue<Vector3> circlePath = new();
        for (int i = 0; i < sampleDensity; i++)
        {

            if (NavMesh.SamplePosition(Quaternion.AngleAxis(sampleDensity / 45 * i, navMeshAgent.transform.up) * dir, out NavMeshHit hit, navMeshAgent.radius, navMeshAgent.areaMask))
            {
                circlePath.Enqueue(hit.position);
            }
        }
        return circlePath;
    }

    private bool RegularNavigate(Vector3 destination)
    {

        bool shouldUpdateDestination =
            !Mathf.Approximately(lastDestination.x, destination.x) ||
            !Mathf.Approximately(lastDestination.y, destination.y) ||
            !Mathf.Approximately(lastDestination.z, destination.z);
        lastDestination = destination;
        if (shouldUpdateDestination) navMeshAgent.SetDestination(destination);

        // Get world space desired velocity from NavMeshAgent
        Vector3 worldDesiredVelocity = navMeshAgent.desiredVelocity;

        // Convert world velocity to local space for animator
        Vector3 localDesiredVelocity = Self.Value.transform.InverseTransformDirection(worldDesiredVelocity);

        float desiredSpeedX = localDesiredVelocity.x;  // Strafe
        float desiredSpeedZ = localDesiredVelocity.z;  // Forward
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);

        animator.SetFloat(XHash, MathF.Round(Mathf.Lerp(
                currentSpeedX,
                desiredSpeedX,
                navMeshAgent.acceleration * Time.deltaTime
            ), 2));
        animator.SetFloat(YHash, MathF.Round(Mathf.Lerp(
                currentSpeedZ,
                desiredSpeedZ,
                navMeshAgent.acceleration * Time.deltaTime
            ), 2));

        if (animator.deltaPosition.magnitude > minMoveDistance)
        {
            // Delta position is already in world space
            Vector3 worldVelocity = animator.deltaPosition / Time.deltaTime;

            // Convert world velocity to local space for NavMeshAgent
            Vector3 localVelocity = Self.Value.transform.InverseTransformDirection(worldVelocity);

            navMeshAgent.velocity = localVelocity;
        }
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f;
    }
}

