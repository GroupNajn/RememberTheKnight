using Unity.Behavior;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;


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

    public RarityTier tier;
    private EnemyVFX enemyVFX;
    private List<Transform> childObjects;

    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        if (CanTakeDamage && Health > 0)
        {
            Event_System.instance.OnEnemyDamage?.Invoke(transform, damage);
            Health -= damage;


            enemyVFX.PlayBloodSplatter(contactPoint);


            OnHealthChanged?.Invoke(Health, MaxHealth);

            CanTakeDamage = false;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        onDeath?.Trigger();
        Event_System.instance.OnEnemyKilled?.Invoke(this);
        childObjects.ForEach(transform => transform.gameObject.layer = 12);

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
        childObjects = GetComponentsInChildren<Transform>().ToList();
       
    }
}