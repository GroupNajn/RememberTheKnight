using FMODUnity;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    // Made by Lukas 2026-03-20
    ProjectileHandler projectileHandler;
    public EventReference rangedShootEvent;

    void Awake()
    {
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    // Theo please call this method in the behavior tree
    public void Attack()
    {
        RuntimeManager.PlayOneShotAttached(rangedShootEvent, gameObject);
        projectileHandler.ShootProjectile();
    }
}