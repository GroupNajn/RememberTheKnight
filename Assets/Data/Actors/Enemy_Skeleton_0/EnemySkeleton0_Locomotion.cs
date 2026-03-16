using System;
using Unity.Behavior;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorGraphAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyLocomotion : MonoBehaviour
{
    void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorAgent.BlackboardReference.GetVariable("stoppingDistance", out stoppingDistance))
            stoppingDistance.Value = navAgent.stoppingDistance;

        if (behaviorAgent.BlackboardReference.GetVariable("agentSpeed", out agentSpeed))
            agentSpeed.Value = navAgent.speed;

    }
    void Update()
    {
        float moveSpeed = animator.GetFloat("MovementSpeed");
        float moveMagnitude = animator.GetFloat("SpeedMagnitude");
        float result = Mathf.Lerp(moveSpeed, moveMagnitude, 0.05f * navAgent.acceleration);
        animator.SetFloat("MovementSpeed", MathF.Round(result, 1));



        stoppingDistance.Value = navAgent.stoppingDistance;
        agentSpeed.Value = navAgent.speed;
    }
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
    private BlackboardVariable<float> agentSpeed;
    private BlackboardVariable<float> stoppingDistance;
}
