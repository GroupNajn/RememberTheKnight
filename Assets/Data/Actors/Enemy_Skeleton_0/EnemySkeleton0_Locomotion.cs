using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyLocomotion : MonoBehaviour
{
    void Awake()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorAgent.BlackboardReference.GetVariable("attackRadius", out BlackboardVariable<float> attackRadius))
        {
            attackRadius.Value = navAgent.stoppingDistance;
        }

    }


    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walking", navAgent.velocity.magnitude > 0);

    }
    private Animator animator;
    private NavMeshAgent navAgent;
    private BehaviorGraphAgent behaviorAgent;
}
