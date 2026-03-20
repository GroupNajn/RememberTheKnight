using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    // Made by Lukas 2026-03-20
    ProjectileHandler projectileHandler;

    void Awake()
    {
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    // Theo please call this method in the behavior tree
    public void Attack()
    {
        projectileHandler.ShootProjectile();
    }
}