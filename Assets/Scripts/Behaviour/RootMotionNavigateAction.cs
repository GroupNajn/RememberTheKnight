using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RootMotionNavigate", story: "[Self] navigates to [Target] using root motion", category: "Action", id: "cb956d9f42fb28ab5eb2287131e6b291")]
public partial class RootMotionNavigateAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<bool> IsNavigating = new(false);
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private Vector3 lastTargetPos;

    protected override Status OnStart()
    {
        animator = Self.Value.GetComponent<Animator>();
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null || animator == null)
        {
            return Status.Failure;
        }

        if (!navMeshAgent.isOnNavMesh) return Status.Failure;

        var dist = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);
        if (dist <= navMeshAgent.stoppingDistance) return Status.Success;

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.SetDestination(Target.Value.transform.position);
        lastTargetPos = Target.Value.transform.position;
        IsNavigating.Value = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (animator == null || navMeshAgent == null) return Status.Failure;
        if (!navMeshAgent.isOnNavMesh) return Status.Failure;
        if (navMeshAgent.hasPath && navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) return Status.Failure;

        bool shouldUpdateDestination =
            !Mathf.Approximately(lastTargetPos.x, Target.Value.transform.position.x) ||
            !Mathf.Approximately(lastTargetPos.y, Target.Value.transform.position.y) ||
            !Mathf.Approximately(lastTargetPos.z, Target.Value.transform.position.z);

        if (shouldUpdateDestination) navMeshAgent.SetDestination(Target.Value.transform.position);
        lastTargetPos = Target.Value.transform.position;

        bool shouldBreak = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position) <= navMeshAgent.stoppingDistance;


        float desiredSpeed = Mathf.Max(navMeshAgent.desiredVelocity.magnitude, navMeshAgent.speed / 2);
        float currentSpeed = animator.GetFloat("MovementSpeed");
        animator.SetFloat("MovementSpeed", MathF.Round(Mathf.Lerp(currentSpeed, desiredSpeed, 0.5f * Time.fixedDeltaTime), 2));

        if (animator.deltaPosition.magnitude > 0.01f) navMeshAgent.velocity = animator.deltaPosition / Time.fixedDeltaTime;

        Vector3 direction = (navMeshAgent.steeringTarget - Self.Value.transform.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        Self.Value.transform.rotation = Quaternion.RotateTowards(
            Self.Value.transform.rotation,
            desiredRotation,
            navMeshAgent.angularSpeed * Time.fixedDeltaTime
        );

        if (shouldBreak)
        {
            currentSpeed = animator.GetFloat("MovementSpeed");
            float newSpeed = Mathf.Max(currentSpeed - 0.1f * Time.deltaTime, 0f);
            animator.SetFloat("MovementSpeed", newSpeed);

            if (!Mathf.Approximately(animator.GetFloat("MovementSpeed"), 0f)) return Status.Success;
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
            animator.SetFloat("MovementSpeed", 0);
        }
        IsNavigating.Value = false;
    }


}

