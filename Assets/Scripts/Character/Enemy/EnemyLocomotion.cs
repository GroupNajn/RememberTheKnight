using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This is a glue component that handles animation events from the <see cref="Animator"/>,
/// sets the velocity of the <see cref="NavMeshAgent"/> based on the root motion of the <see cref="Animator"/>,
/// adjusts the simulated position of the <see cref="NavMeshAgent"/> so it aligns with the real position,
/// feeds the <see cref="BehaviorGraphAgent"/> with data about the <see cref="NavMeshAgent"/> that is needed for correct positioning
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyLocomotion : MonoBehaviour
{
    // sets the IsHeavy variable, used by EnemyWeaponManager to decide whther to use Light or Heavy Attack damage
    private static readonly int IsHeavyHash = Animator.StringToHash("IsHeavy");
    // set to randomize the attack pattern used by enemy
    private static readonly int RandomHash = Animator.StringToHash("Random");
    // the speed in the world Z axis used by the Animators 2D blend tree for locomotion
    private static readonly int YHash = Animator.StringToHash("Y");
    // the speed in the world X axis used by the Animators 2D blend tree for locomotion
    private static readonly int XHash = Animator.StringToHash("X");
    // used by Animator and BehaviorGraph to know if a non damaging action is being executed
    private static readonly int IsEmotingHash = Animator.StringToHash("IsEmoting");
    // used by Animator and BehaviorGraph to know if a damaging action is being executed
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();

        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

        // the furthest distance the enemy can to be to execute a melee attack 
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

    // Animation event that runs at the start of each enemy attack
    public void OnAttackStart() => animator.SetBool(IsAttackingHash, true);

    // Animation event that runs at the start of each enemy heavy attack
    public void OnHeavyAttack() => animator.SetBool(IsHeavyHash, true);
    // Animation event that runs at the end of each enemy (light or heavy) attack
    public void OnAttackEnd()
    {
        animator.SetBool(IsAttackingHash, false);
        animator.SetBool(IsHeavyHash, false);
        animator.SetFloat(RandomHash, UnityEngine.Random.value);
    }
    //  Animation event that runs at the start of each enemy non damaging action
    public void OnEmoteStart() => animator.SetBool(IsEmotingHash, true);

    //  Animation event that runs at the end of each enemy non damaging action
    public void OnEmoteEnd() => animator.SetBool(IsEmotingHash, false);
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
    private BlackboardVariable<float> stoppingDistance;


}
