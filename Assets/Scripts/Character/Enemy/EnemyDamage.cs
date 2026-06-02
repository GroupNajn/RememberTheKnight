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
    private static readonly int HitHash = Animator.StringToHash("Hit");

    //[SerializeField] private Event_System EventSystem;
    // Made by Lukas and Anton B 2026-03-06
    //Edited by Michaëla 2026-05-06
    [field: SerializeField] public float MaxHealth { get; set; }
    private float healthField;
    public float Health
    {
        get => healthField;
        set
        {
            bool healing = value > healthField;
            healthField = value;
            OnHealthChanged?.Invoke(Health, MaxHealth, healing);
        }
    }
    public Action<float, float, bool> OnHealthChanged { get; set; }
    [SerializeField, Tooltip("When unaware of player incoming damage is multiplied by this value")] public float SneakMultiplier = 1.5f;
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;

    [SerializeField] float healthModifierPercentagePerLevel = 1.2f;

    public RarityTier tier;
    private EnemyVFX enemyVFX;
    private CharacterSoundFXManager enemySFX;
    private Animator animator;
    private List<Transform> childObjects;
    private BlackboardVariable<bool> hasSight;
    private BlackboardVariable<bool> hasAggro;
    private BehaviorGraphAgent behaviorGraphAgent;
    private GameData gameData;


    public void SetHealthModifier(int level)
    {
        float originalMaxHealth = MaxHealth;

        // Set the new max health with an exponentioal scaling based on current level number
        MaxHealth *= Mathf.Pow(healthModifierPercentagePerLevel, level);
        Health = MaxHealth;
    }

    public void TakeDamage(DamageInfo damageInfo, Vector3 contactPoint)
    {
        if (CanTakeDamage && Health > 0)
        {
            if (hasSight != null && hasAggro != null && !hasSight.Value && !hasAggro.Value)
            {
                damageInfo.SetDamageAmount(damageInfo.DamageAmount * SneakMultiplier);
                damageInfo.IsSneak = true;
            }
            else
            {
                damageInfo.SetDamageAmount(damageInfo.DamageAmount);
            }

            Health -= damageInfo.DamageAmount;

            Event_System.instance.OnEnemyDamage?.Invoke(transform, damageInfo);
            enemyVFX.PlayBloodSplatter(contactPoint);
            enemySFX.PlayDamageGrunt();
            animator.SetTrigger(HitHash);

            if (Health <= 0)
            {
                enemySFX.PlayDeathSoundFX();
                Death();
            }
        }
    }

    public void Death()
    {
        EnemyLootProfile profile = gameObject.GetComponent<EnemyLootProfile>();
        Event_System.instance.OnEnemyKilledNew?.Invoke(profile, this.transform.position);
        childObjects.ForEach(transform => transform.gameObject.layer = 12);
        gameData.TotalEnemiesSlain++;
        
        onDeath?.Trigger();
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
        gameData = GameObject.Find("GlobalData").GetComponent<GameData>();
        if (behaviorGraphAgent.BlackboardReference.GetVariable("Has Sight", out hasSight)) { }
        if (behaviorGraphAgent.BlackboardReference.GetVariable("Has Aggro", out hasAggro)) { }
    }
}