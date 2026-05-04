using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RootMotionNavigate", story: "[Self] navigates to [Target] using root motion", category: "Action", id: "cb956d9f42fb28ab5eb2287131e6b291")]
public partial class RootMotionNavigateAction : Action
{
    private static readonly int MovementSpeedHash = Animator.StringToHash("MovementSpeed");
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<bool> IsNavigating = new(false);

    [SerializeReference] public BlackboardVariable<List<string>> BreakingEmotes = new(new());
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;

    private Vector3 lastTargetPos;

    protected override Status OnStart()
    {
        animator = Self.Value.GetComponent<Animator>();
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        characterController = Self.Value.GetComponent<CharacterController>();
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

        navMeshAgent.nextPosition = Self.Value.transform.position;

        bool shouldUpdateDestination =
            !Mathf.Approximately(lastTargetPos.x, Target.Value.position.x) ||
            !Mathf.Approximately(lastTargetPos.y, Target.Value.position.y) ||
            !Mathf.Approximately(lastTargetPos.z, Target.Value.position.z);


        if (shouldUpdateDestination) navMeshAgent.SetDestination(Target.Value.position);
        lastTargetPos = Target.Value.position;


        bool isEmoting = false;
        var animState = animator.GetCurrentAnimatorStateInfo(0);

        foreach (var breakingEmote in BreakingEmotes.Value)
        {
            if (animState.IsName(breakingEmote))
                isEmoting = true;
        }

        float minSpeed = characterController != null ? characterController.minMoveDistance / Time.deltaTime : 0.01f;

        if (!isEmoting)
        {

            float desiredSpeed = Mathf.Max(navMeshAgent.desiredVelocity.magnitude, minSpeed);
            float currentSpeed = animator.GetFloat(MovementSpeedHash);
            animator.SetFloat(MovementSpeedHash, MathF.Round(Mathf.Lerp(currentSpeed, desiredSpeed, navMeshAgent.acceleration * Time.deltaTime), 2));
            if (animator.deltaPosition.magnitude > 0.01f) navMeshAgent.velocity = animator.deltaPosition / (Time.deltaTime + 0.00001f);
        }
        else
        {
            animator.SetFloat(MovementSpeedHash, 0f);
            navMeshAgent.velocity = Vector3.zero;

        }

        Vector3 direction = navMeshAgent.steeringTarget - navMeshAgent.nextPosition;
        direction.Normalize();
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        if (Quaternion.Angle(Self.Value.transform.rotation, desiredRotation) > 5)
        {
            Self.Value.transform.rotation = Quaternion.RotateTowards(
                Self.Value.transform.rotation,
                desiredRotation,
                navMeshAgent.angularSpeed * Time.deltaTime
            );
        }

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            return Status.Success;
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
            animator.SetFloat(MovementSpeedHash, 0);
        }
        IsNavigating.Value = false;
    }


}

