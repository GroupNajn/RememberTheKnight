using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyLocomotion : MonoBehaviour
{
    private static readonly int IsHeavyHash = Animator.StringToHash("IsHeavy");
    private static readonly int RandomHash = Animator.StringToHash("Random");
    private static readonly int YHash = Animator.StringToHash("Y");
    private static readonly int XHash = Animator.StringToHash("X");
    private static readonly int IsEmotingHash = Animator.StringToHash("IsEmoting");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

        if (behaviorAgent.BlackboardReference.GetVariable<float>("attackRadius", out var attackRadius))
            attackRadius.Value = navAgent.stoppingDistance * 2f;


        animator.SetFloat(RandomHash, UnityEngine.Random.value);
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
    }

    public void OnAttackStart() => animator.SetBool(IsAttackingHash, true);
    public void OnHeavyAttack() => animator.SetBool(IsHeavyHash, true);
    public void OnAttackEnd()
    {
        animator.SetBool(IsAttackingHash, false);
        animator.SetBool(IsHeavyHash, false);
        animator.SetFloat(RandomHash, UnityEngine.Random.value);
    }
    public void OnEmoteStart() => animator.SetBool(IsEmotingHash, true);
    public void OnEmoteEnd() => animator.SetBool(IsEmotingHash, false);
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
    private BlackboardVariable<float> stoppingDistance;


}
