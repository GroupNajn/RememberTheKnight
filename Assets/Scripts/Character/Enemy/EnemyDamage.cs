using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
[RequireComponent(typeof(ITriggerable))]


[RequireComponent(typeof(EnemyVFX))]
[RequireComponent(typeof(CharacterSoundFXManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BehaviorGraphAgent))]
public class EnemyDamage : MonoBehaviour, IDamageable
{
    private static readonly int HasAggroHash = Animator.StringToHash("HasAggro");
    private static readonly int HasSightHash = Animator.StringToHash("HasSight");
    private static readonly int HitHash = Animator.StringToHash("Hit");

    //[SerializeField] private Event_System EventSystem;
    // Made by Lukas and Anton B 2026-03-06
    //Edited by Michaëla 2026-05-06
    [field: SerializeField] public float MaxHealth { get; set; }
    [HideInInspector] public float Health { get; set; }
    public Action<float, float> OnHealthChanged { get; set; }
    [SerializeField, Tooltip("When current threat is zero incoming damage is multiplied by this value")] public float SneakMultiplier = 1.5f;
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;
    float damageCooldownTimer;
    [SerializeField] float damageCooldown = 1;

    [SerializeField] float healthModifierPercentagePerLevel = 1.2f;

    public RarityTier tier;
    private EnemyVFX enemyVFX;
    private CharacterSoundFXManager enemySFX;
    private Animator animator;
    private List<Transform> childObjects;
    private BlackboardVariable<bool> hasSight;
    private BlackboardVariable<bool> hasAggro;
    private BehaviorGraphAgent behaviorGraphAgent;


    public void SetHealthModifier(int level)
    {
        float originalMaxHealth = MaxHealth;

        MaxHealth *= Mathf.Pow(healthModifierPercentagePerLevel, level);
        Health = MaxHealth;
        OnHealthChanged?.Invoke(Health, MaxHealth);
    }

    public void TakeDamage(DamageInfo damageInfo, Vector3 contactPoint)
    {
        if (CanTakeDamage && Health > 0)
        {
            if (!hasSight.Value && !hasAggro.Value)
            {
                damageInfo.SetDamageAmount(damageInfo.DamageAmount * SneakMultiplier);
                damageInfo.IsSneak = true;
            }
            Health -= damageInfo.DamageAmount;
            OnHealthChanged?.Invoke(Health, MaxHealth);

            Event_System.instance.OnEnemyDamage?.Invoke(transform, damageInfo);
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
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        if (behaviorGraphAgent.BlackboardReference.GetVariable("Has Sight", out hasSight)) { }
        if (behaviorGraphAgent.BlackboardReference.GetVariable("Has Aggro", out hasAggro)) { }


    }
}