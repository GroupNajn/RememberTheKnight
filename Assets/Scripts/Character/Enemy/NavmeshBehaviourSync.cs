using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]
public class NavmeshBehaviourSync : MonoBehaviour
{
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorAgent.BlackboardReference.GetVariable("isAttacking", out isAttacking)) { }
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

    }

    public void OnAttackStart() { isAttacking.Value = true; }
    public void OnAttackEnd() { isAttacking.Value = false; }
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
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

    private BlackboardVariable<bool> isAttacking;
}
