using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Root Motion Navigate", story: "[Self] navigates to [Target] using root motion with avoidance priotity [Priority]", category: "Action", id: "cb956d9f42fb28ab5eb2287131e6b291")]
public partial class RootMotionNavigateAction : Action
{
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<int> Priority;
    [SerializeReference] public BlackboardVariable<float> CircleRadius;
    [SerializeReference] public BlackboardVariable<bool> ShouldStopAtCircleRadius = new(false);
    [SerializeReference] public BlackboardVariable<List<string>> BreakingEmotes = new(new());
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private CharacterController characterController;

    private Vector3 targetPos;
    private Vector3 lastTargetPos;
    private float minMoveDistance;
    private int initialAvoidancePriority;
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
        navMeshAgent.updateRotation = true;
        if (navMeshAgent.hasPath) navMeshAgent.ResetPath();
        if (ShouldStopAtCircleRadius.Value)
            targetPos = SampleCirclePoints(10);
        else
            targetPos = Target.Value.position;

        navMeshAgent.SetDestination(targetPos);
        lastTargetPos = Target.Value.position;
        navMeshAgent.avoidancePriority = Priority.Value;
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
        if (shouldUpdateDestination)
        {
            if (ShouldStopAtCircleRadius.Value)
                navMeshAgent.SetDestination(SampleCirclePoints(10));
            else
                navMeshAgent.SetDestination(Target.Value.position);
        }

        Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
        Vector3 desiredLocalVelocity = Vector3.zero;
        if (desiredVelocity != Vector3.zero)
            desiredLocalVelocity = Self.Value.transform.InverseTransformDirection(desiredVelocity).normalized;

        float desiredSpeedX = desiredLocalVelocity.x;
        float desiredSpeedZ = desiredLocalVelocity.z;
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);

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

    protected override void OnEnd()
    {

        navMeshAgent.avoidancePriority = initialAvoidancePriority;
        if (navMeshAgent.isOnNavMesh) navMeshAgent.ResetPath();
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

            if (NavMesh.SamplePosition(Quaternion.AngleAxis(sampleDensity / 360 * i, Vector3.up) * dir + Target.Value.position, out NavMeshHit hit, navMeshAgent.radius, navMeshAgent.areaMask))
            {
                return hit.position;
            }
        }
        return navMeshAgent.transform.position;
    }
}

