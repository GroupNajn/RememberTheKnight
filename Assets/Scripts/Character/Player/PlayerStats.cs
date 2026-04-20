using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // Made by Lukas 2026-03-14
    // Updated by Lukas and Jonatan and Wilmer 2026-03-16

    private PlayerCombatManager playerCombatManager;
    private Animator playerAnimator;
    private PlayerVFX playerVFX;

    [Header("Player Stats")]

    [Header("Health")]
    public float MaxHealth;
    public float CurrentHealth;
    public float healthRegenRate = 0f;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 1.5f;

    [Header("Luck")]
    public float currentLuck;

    [Header("Damage")]
    public float damageMultiplier;

    [Header("Crit")]
    public float critChance;

    [Header("Movement Stats")]
    [Header("Movement")]
    public float walkSpeedMultiplier = 0f;
    public float sprintSpeedMultiplier = 0f;
    public float gravity = 25f;
    public float normalRotationSpeed = 10f;
    public float attackRotationSpeed = 5f;

    [Header("Dodge")]
    public float dodgeSpeedMultiplier = 0f;
    public float dodgeCoolDown = 0.5f;
    public float dodgeDuration = 0.2f;

    [Header("Knockback")]
    public float knockbackResistance = 5f;

    [Header("Multipliers")]
    public float healMultiplier = 1;

    [Header("Flags")]
    [SerializeField] public bool isDead = false;

    // Base Values used for Applying Stats
    public float baseHealth;
    public float baseStamina;
    public float baseLuck;

    [HideInInspector]
    public bool CanTakeDamage
    {
        get { return !playerCombatManager.isInvulnerable; }
        private set { }
    }

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        playerAnimator = GetComponent<Animator>();
        playerVFX = GetComponentInChildren<PlayerVFX>();
        CurrentHealth = MaxHealth;
        baseHealth = MaxHealth;
        baseStamina = maxStamina;
        currentStamina = maxStamina;
        currentStamina = maxStamina;
        //Event_System.instance.OnStatsApplied += ApplyStatsFromCardSelection;
    }

    //private void OnDisable()
    //{
    //    if(Event_System.instance != null)
    //    {
    //    Event_System.instance.OnStatsApplied -= ApplyStatsFromCardSelection;
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (Event_System.instance != null)
    //    {
    //        Event_System.instance.OnStatsApplied += ApplyStatsFromCardSelection;
    //    }
    //}

    //public void ApplyStatsFromCardSelection(List<CardData> cards)
    //{
    //    MaxHealth = baseHealth;
    //    maxStamina = baseStamina;
    //    foreach (CardData card in cards)
    //    {
    //        if (card == null) continue;
    //        MaxHealth += card.healthModifier;
    //        maxStamina += card.staminaModifier;
    //    }
    //    Health = MaxHealth;
    //    currentStamina = maxStamina;
    //}
}