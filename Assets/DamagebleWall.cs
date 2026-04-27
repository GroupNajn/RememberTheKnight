using System;
using UnityEngine;

public class DamagebleWall : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; set; }
    [HideInInspector] public float Health { get; set; }
    public Action<float, float> OnHealthChanged { get; set; }

    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;
    float damageCooldownTimer;
    [SerializeField] float damageCooldown = 1;

    private EnemyVFX enemyVFX;

    private void Start()
    {
        enemyVFX = GetComponent<EnemyVFX>();
        Health = MaxHealth;
        onDeath = GetComponent<ITriggerable>();
    }


    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        if (CanTakeDamage && Health > 0)
        {
            Health -= damage;

            enemyVFX.PlayBloodSplatter(contactPoint);

            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
