using Unity.Behavior;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using System;


[RequireComponent(typeof(ITriggerable))]
public class EnemyDamage : MonoBehaviour, IDamageable
{
    //[SerializeField] private Event_System EventSystem;
    // Made by Lukas and Anton B 2026-03-06
    [field: SerializeField] public float MaxHealth { get; set; }
    [HideInInspector] public float Health { get; set; }
    public Action<float, float> OnHealthChanged { get; set; }
    
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;
    float damageCooldownTimer;
    [SerializeField] float damageCooldown = 1;

    private EnemyVFX enemyVFX;

    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        if (CanTakeDamage && Health > 0)
        {
            Debug.Log($"Taking damage{damage}");
            Event_System.instance.OnEnemyDamage?.Invoke(this.transform,damage);
            Health -= damage;


            enemyVFX.PlayBloodSplatter(contactPoint);


            OnHealthChanged?.Invoke(Health, MaxHealth);

            Debug.Log($"Health {Health}/{MaxHealth}");
            CanTakeDamage = false;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Debug.Log("Enemy died");
        onDeath?.Trigger();
        Debug.Log("Invoking OnEnemyKilled");
        Event_System.instance.OnEnemyKilled?.Invoke(this);

    }

    // TO BE REMOVED OR CHANGED
    void Update()
    {
        if (!CanTakeDamage)
        {

            damageCooldownTimer -= Time.deltaTime;
            if (damageCooldownTimer <= 0)
            {
                CanTakeDamage = true;
                damageCooldownTimer = damageCooldown;
            }
        }
    }

    private void Start()
    {
        enemyVFX = GetComponent<EnemyVFX>();
        Health = MaxHealth;
        onDeath = GetComponent<ITriggerable>();
        if (Event_System.instance != null)
            Event_System.instance.OnEnemySpawn?.Invoke(this);

        else
            Debug.LogError("Event_System.instance is null in EnemyDamage.Start()");
    }
}