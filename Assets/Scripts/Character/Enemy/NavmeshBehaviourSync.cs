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
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

    }
    void Update()
    {/*
        float moveSpeed = animator.GetFloat("MovementSpeed");
        float moveMagnitude = animator.GetFloat("SpeedMagnitude");
        float result = Mathf.Lerp(moveSpeed, moveMagnitude, 0.05f * navAgent.acceleration);
        animator.SetFloat("MovementSpeed", MathF.Round(result, 1));
        agentSpeed.Value = navAgent.speed;
        */
    }
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
    private BlackboardVariable<float> stoppingDistance;
}
