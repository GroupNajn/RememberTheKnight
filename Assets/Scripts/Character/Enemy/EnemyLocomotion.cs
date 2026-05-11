using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(CharacterController))]
public class EnemyLocomotion : MonoBehaviour
{
    private static readonly int IsEmotingHash = Animator.StringToHash("IsEmoting");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

        if (behaviorAgent.BlackboardReference.GetVariable<float>("attackRadius", out var attackRadius))
            attackRadius.Value = navAgent.stoppingDistance * 2f;

    }

    void Update()
    {
        if (animator.deltaPosition.magnitude > characterController.minMoveDistance)
        {
            navAgent.velocity = animator.deltaPosition / Time.deltaTime;
        }
        navAgent.nextPosition = transform.position;
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
