using Unity.Behavior;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
[RequireComponent(typeof(ITriggerable))]


[RequireComponent(typeof(EnemyVFX))]
[RequireComponent(typeof(CharacterSoundFXManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyDamage : MonoBehaviour, IDamageable
{
    private static readonly int HitHash = Animator.StringToHash("Hit");

    //[SerializeField] private Event_System EventSystem;
    // Made by Lukas and Anton B 2026-03-06
    //Edited by Michaëla 2026-05-06
    [field: SerializeField] public float MaxHealth { get; set; }
    [HideInInspector] public float Health { get; set; }
    public Action<float, float> OnHealthChanged { get; set; }
    [SerializeField, Tooltip("When current threat is zero incoming damage is multiplied by this value")] public float SneakMultiplier = 1.3f;
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;
    float damageCooldownTimer;
    [SerializeField] float damageCooldown = 1;

    public RarityTier tier;
    private EnemyVFX enemyVFX;
    private CharacterSoundFXManager enemySFX;
    private Animator animator;
    private BlackboardVariable<float> threat;
    private List<Transform> childObjects;

    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        float incomingDamage = damage;
        if (CanTakeDamage && Health > 0)
        {
            if (Mathf.Approximately(threat.Value, 0)) incomingDamage *= SneakMultiplier;
            
            Health -= incomingDamage;
            OnHealthChanged?.Invoke(Health, MaxHealth);
            
            Event_System.instance.OnEnemyDamage?.Invoke(transform, incomingDamage);
            
            enemyVFX.PlayBloodSplatter(contactPoint);
            enemySFX.PlayDamageGrunt();
            animator.SetTrigger(HitHash);

            CanTakeDamage = false;
            if (Health <= 0)
            {
                enemySFX.PlayDeathSoundFX();
                Death();
            }
        }
    }

    public void Death()
    {
        onDeath?.Trigger();
        //Event_System.instance.OnEnemyKilled?.Invoke(this);

        EnemyLootProfile profile = gameObject.GetComponent<EnemyLootProfile>();
        Event_System.instance.OnEnemyKilledNew?.Invoke(profile, this.transform.position);
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
        enemySFX = GetComponent<CharacterSoundFXManager>();
        Health = MaxHealth;
        onDeath = GetComponent<ITriggerable>();
        childObjects = GetComponentsInChildren<Transform>().ToList();
        animator = GetComponent<Animator>();
        if (GetComponent<BehaviorGraphAgent>().BlackboardReference.GetVariable<float>("currentThreat", out threat)) { }


    }
}