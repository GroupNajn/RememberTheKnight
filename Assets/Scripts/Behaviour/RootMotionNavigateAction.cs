using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Root Motion Navigate", story: "[Self] navigates to [Target] using root motion", category: "Action", id: "cb956d9f42fb28ab5eb2287131e6b291")]
public partial class RootMotionNavigateAction : Action
{
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<bool> IsNavigating = new(false);

    [SerializeReference] public BlackboardVariable<List<string>> BreakingEmotes = new(new());
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;

    private Vector3 lastTargetPos;
    private float minMoveDistance;
    protected override Status OnStart()
    {
        animator = Self.Value.GetComponent<Animator>();
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        characterController = Self.Value.GetComponent<CharacterController>();
        minMoveDistance = characterController != null ? characterController.minMoveDistance : 0.01f;
        if (navMeshAgent == null || animator == null)
        {
            return Status.Failure;
        }

        if (!navMeshAgent.isOnNavMesh) return Status.Failure;

        var dist = Vector3.Distance(Self.Value.transform.position, Target.Value.position);
        if (dist <= navMeshAgent.stoppingDistance) return Status.Success;

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        navMeshAgent.SetDestination(Target.Value.position);
        lastTargetPos = Target.Value.position;
        IsNavigating.Value = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (animator == null || navMeshAgent == null) return Status.Failure;
        if (!navMeshAgent.isOnNavMesh) return Status.Failure;
        if (navMeshAgent.hasPath && navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) return Status.Failure;
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance) return Status.Success;
        if (Time.deltaTime <= 1e-5f) return Status.Running;

        bool shouldUpdateDestination =
            !Mathf.Approximately(lastTargetPos.x, Target.Value.position.x) ||
            !Mathf.Approximately(lastTargetPos.y, Target.Value.position.y) ||
            !Mathf.Approximately(lastTargetPos.z, Target.Value.position.z);
        lastTargetPos = Target.Value.position;
        navMeshAgent.nextPosition = Self.Value.transform.position;
        if (shouldUpdateDestination) navMeshAgent.SetDestination(Target.Value.position);

        float desiredSpeedX = navMeshAgent.desiredVelocity.x;
        float desiredSpeedZ = navMeshAgent.desiredVelocity.z;
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
            navMeshAgent.velocity = animator.deltaPosition / Time.deltaTime;

        Vector3 direction = (navMeshAgent.steeringTarget - navMeshAgent.nextPosition).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(direction, Self.Value.transform.up);
        if (Quaternion.Angle(Self.Value.transform.rotation, desiredRotation) > 5)
        {
            Self.Value.transform.rotation = Quaternion.RotateTowards(
                Self.Value.transform.rotation,
                desiredRotation,
                navMeshAgent.angularSpeed * Time.deltaTime
            );
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (navMeshAgent != null)
        {
            if (navMeshAgent.isOnNavMesh) navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
        }
        if (animator != null)
        {
            animator.SetFloat(XHash, 0);
            animator.SetFloat(YHash, 0);
        }
        IsNavigating.Value = false;
    }
}

