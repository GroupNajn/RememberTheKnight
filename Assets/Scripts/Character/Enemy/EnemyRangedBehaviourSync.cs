using System.Linq;
using Unity.Behavior;
using UnityEngine;

/// <summary>
/// This is a glue component that syncs ranged enemies <see cref="Animator"/>, <see cref="BehaviorGraphAgent"/> and <see cref="ProjectileHandler"/>.
/// Handles the event OnCast the <see cref="Animator"/> to shoot projectiles,
/// Makes sure the <see cref="ProjectileHandler"/> shoots the central body of the player
/// </summary>
/// <remarks>Author: Theo Johansson</remarks>
[RequireComponent(typeof(BehaviorGraphAgent), typeof(ProjectileHandler), typeof(Animator))]
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

    void Update()
    {
        if (behaviorAgent.BlackboardReference.GetVariable<GameObject>("Target", out var target))
        {
            if (target.Value == null) return;
            if (target.Value != previousTarget)
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

    // Animation event that runs when it is time to fire a projectile
    public void OnCast() { projectileHandler.ShootProjectile(); }

}
