using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour, IDamageable
{
    // Made by Lukas 2026-03-14
    // Updated by Lukas and Jonatan and Wilmer 2026-03-16

    private PlayerCombatManager playerCombatManager;
    private Animator playerAnimator;
    private PlayerVFX playerVFX;

    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float Health { get; set; }
    public System.Action<float, float> OnHealthChanged { get; set; }



    float healMultiplier = 1;

    [Header("Stats")]
    [Header("Movement")]
    public float walkSpeedMultiplier = 0f;
    public float sprintSpeedMultiplier = 0f;
    public float gravity = 25f;
    public float normalRotationSpeed = 10f;
    public float attackRotationSpeed = 5f;
    [Header("Dodge")]
    public float dodgeSpeedMultiplier = 0f;
    public float dodgeCoolDown = 1f;
    public float dodgeDuration = 0.2f;
    [Header("Knockback")]
    public float knockbackResistance = 5f;

    public System.Action<float, float> onStaminaChange;
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 1.5f;

    // Base Values used for Applying Stats
    private float baseHealth;
    private float baseStamina;
    // [HideInInspector]

    [HideInInspector]

    public bool CanTakeDamage
    {
        get { return !playerCombatManager.isInvulnerable; }
        private set { }
    }

    [SerializeField] public bool isDead = false;

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        playerAnimator = GetComponent<Animator>();
        playerVFX = GetComponentInChildren<PlayerVFX>();
        Health = MaxHealth;
        baseHealth = MaxHealth;
        baseStamina = maxStamina;
        currentStamina = maxStamina;
        currentStamina = maxStamina;
        Event_System.instance.OnStatsApplied += ApplyStats;
    }

    void ApplyStats(List<CardData> cards)
    {
        MaxHealth = baseHealth;
        maxStamina = baseStamina;
        foreach (CardData card in cards)
        {
            if (card == null) continue;
            MaxHealth += card.healthModifier;
            maxStamina += card.staminaModifier;
        }
        Health = MaxHealth;
        currentStamina = maxStamina;


    }

    private void Update()
    {
        if (Health <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        playerAnimator.SetBool("IsDead", isDead);

    }

    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        if (CanTakeDamage && !isDead)
        {
            playerVFX.PlayBloodSplatter(contactPoint);

            Health -= damage;
            NotifyHealthChanged();
            if (isDead)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Debug.Log("DIE!");
        playerAnimator.SetBool("IsDead", true);
    }

    private void NotifyHealthChanged()
    {

        Debug.Log("EVENT TRIGGERED: " + Health);
        OnHealthChanged?.Invoke(Health, MaxHealth);
    }

    public void NotifyDeath()
    {
        Debug.Log("EVENT TRIGGERED: Player Died");
        Event_System.instance.OnPlayerDeath?.Invoke();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;

        Health = Mathf.Clamp(Health, 0, MaxHealth);

        NotifyHealthChanged();
    }

    public void Heal(float amount)
    {
        float totalHeal = amount * healMultiplier;
        Health = Mathf.Clamp(Health + totalHeal, 0, MaxHealth);
        NotifyHealthChanged();
    }

    public void UpdateMaxStamina(float newMaxStamina)
    {
        maxStamina = newMaxStamina;

        onStaminaChange?.Invoke(currentStamina, maxStamina);
    }


}