using System.Linq;
using Unity.Behavior;
using UnityEngine;

[RequireComponent(typeof(BehaviorGraphAgent), typeof(ProjectileHandler))]
public class EnemyRangedBehaviourSync : MonoBehaviour
{
    private BehaviorGraphAgent behaviorAgent;
    private ProjectileHandler projectileHandler;
    private GameObject previousTarget;
    void Start()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviorAgent.BlackboardReference.GetVariable<GameObject>("Target", out var target))
        {
            if (target.Value != null || target.Value != previousTarget)
            {
                Transform spine = null;
                target.Value.GetComponentsInChildren<Transform>().ToList().ForEach(transform =>
                {
                    if (transform.name == "Spine_02") spine = transform;
                });
                projectileHandler.target = spine != null ? spine : target.Value.transform;
                previousTarget = target.Value;
            }
        }
    }

    public void OnCast() { projectileHandler.ShootProjectile(); }

}
