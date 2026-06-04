using FMODUnity;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-03-20
    /// Simple method to shoot a projectile when attacking
    /// </summary>

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