using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    // Made by Lukas 2026-03-20
    ProjectileHandler projectileHandler;

    void Awake()
    {
        projectileHandler = GetComponent<ProjectileHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Attack");
            Attack();
        }
    }

    // Theo please call this method in the behavior tree
    public void Attack()
    {
        projectileHandler.ShootProjectile();
    }
}