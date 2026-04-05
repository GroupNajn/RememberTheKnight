using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent), typeof(ProjectileHandler))]
public class EnemyRangedBehaviourSync : MonoBehaviour
{
    private BehaviorGraphAgent behaviorAgent;
    private ProjectileHandler projectileHandler;
    void Start()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviorAgent.BlackboardReference.GetVariable<GameObject>("Target", out var target))
            projectileHandler.target = target.Value;
    }

    public void OnCast() { projectileHandler.ShootProjectile(); }
}
