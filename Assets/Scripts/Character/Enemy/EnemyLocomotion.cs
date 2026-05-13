using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(CharacterController))]
public class EnemyLocomotion : MonoBehaviour
{
    [SerializeField] MultiAimConstraint aimConstraint;

    [SerializeField] Transform lookAt;
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    private static readonly int IsEmotingHash = Animator.StringToHash("IsEmoting");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        //lookAt.name = $"{name} {lookAt.name}";
        //lookAt.SetParent(null);
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

        if (behaviorAgent.BlackboardReference.GetVariable<float>("attackRadius", out var attackRadius))
            attackRadius.Value = navAgent.stoppingDistance * 2f;

    }

    void Update()
    {
        float currentSpeedX = animator.GetFloat(XHash);
        float currentSpeedZ = animator.GetFloat(YHash);

        Vector3 localVelocity = new(currentSpeedX, 0f, currentSpeedZ);
        if (localVelocity.magnitude > 1f)
            localVelocity.Normalize();

        navAgent.velocity = transform.TransformDirection(localVelocity) * navAgent.speed;
        navAgent.nextPosition = transform.position;



        // if (aimConstraint == null)
        //     return;

        // if (behaviorAgent.BlackboardReference.GetVariable<GameObject>("Target", out var target))
        // {
        //     if (target.Value)
        //     {
        //         if (currentTarget == null || currentTarget != target.Value)
        //         {
        //             currentTarget = target.Value;
        //             currentAimAt = currentTarget.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.name == "Head");
        //         }
        //     }
        // }

        // if (currentAimAt != null)
        //     lookAt.position = currentAimAt.transform.position;

        // float weightTarget;
        // if (animator.GetBool(IsAttackingHash) || currentAimAt == null) weightTarget = 0;
        // else weightTarget = 1;

        // aimConstraint.weight = Mathf.Lerp(aimConstraint.weight, weightTarget, 4 * Time.deltaTime);
    }

    public void OnAttackStart() => animator.SetBool(IsAttackingHash, true);
    public void OnAttackEnd() => animator.SetBool(IsAttackingHash, false);
    public void OnEmoteStart() => animator.SetBool(IsEmotingHash, true);
    public void OnEmoteEnd() => animator.SetBool(IsEmotingHash, false);
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
    private CharacterController characterController;
    private BlackboardVariable<float> stoppingDistance;
    private GameObject currentTarget;
    private Transform currentAimAt;
    public bool InCombat
    {
        get
        {
            if (behaviorAgent.BlackboardReference.GetVariable("currentThreat", out BlackboardVariable<float> threat))
            {
                return threat.Value > 0.4;
            }
            return false;
        }
    }

}
