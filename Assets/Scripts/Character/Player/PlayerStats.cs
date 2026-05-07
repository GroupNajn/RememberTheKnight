using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // Made by Lukas 2026-03-14
    // Updated by Lukas and Jonatan and Wilmer 2026-03-16

    private PlayerCombatManager playerCombatManager;
    private PlayerWeaponManager playerWeaponManager;
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
    public float staminaRegenDelay = 0.5f;

    [Header("Chance")]
    public float currentLuck = 0;
    public float currentCritChance = 0;

    [Header("Weapon Size")]
    public Vector3 maxWeaponSize = new Vector3(2, 4, 2);
    public Vector3 currentWeaponSize;

    [Header("Modifiers")]
    public float currentDamageModifier = 1;
    public float currentWalkSpeedModifier = 0f;
    public float currentSprintSpeedModifier = 0f;
    public float currentDodgeSpeedModifier = 0f;
    public float currentHealModifier= 1;

    [Header("Knockback")]
    public float currentKnockbackResistance = 5f;


    [Header("Movement Stats")]
    [Header("Movement")]
    public float gravity = 25f;
    public float normalRotationSpeed = 10f;
    public float attackRotationSpeed = 5f;
    [Header("Dodge")]
    public float dodgeCoolDown = 0.5f;
    public float dodgeDuration = 0.2f;

    //[Header("Flags")]
    //[SerializeField] public bool isDead = false;

    [Header("Healing Cup")]
    public int startingCharges = 30;
    public int maxHealingCharges = 100;
    public int currentHealingCharges;
    public int healingChargeCost = 10;
    public float cupHealAmount = 20f;

    // Base Values used for Applying Stats
    [Header("Base Values")]
    public float baseHealth;
    public float baseStamina;
    public float baseLuck;
    public float baseCritChance;
    public float baseWalkSpeedModifier;
    public float baseSprintSpeedModifier;
    public float baseDodgeSpeedModifier;
    public float baseDamageModifier;
    public float baseHealModifier;
    public float baseKnockbackResistance;

    public Vector3 baseWeaponSize;

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

        currentStamina = maxStamina;
        baseStamina = maxStamina;

        baseLuck = currentLuck;
        baseCritChance = currentCritChance;

        baseWalkSpeedModifier = currentWalkSpeedModifier;
        baseSprintSpeedModifier = currentSprintSpeedModifier;
        baseDodgeSpeedModifier = currentDodgeSpeedModifier;
        baseDamageModifier = currentDamageModifier;
        baseHealModifier = currentHealModifier;
        baseKnockbackResistance = currentKnockbackResistance;
        
        baseWeaponSize = new Vector3(1.2f, 1.2f,1.2f);
        currentWeaponSize = baseWeaponSize;
    }
}