using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central controller for player health, stamina,
/// healing, death handling and stat application.
///
/// Implements IDamageable and serves as the main
/// interface between gameplay systems and player stats.
/// </summary>
public class PlayerManager : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Changed by Anton and Theo 2026-05-11
    /// Added functionality to handle vignette when out of stamina.
    /// </summary>
    private PlayerCombatManager playerCombatManager;
    private PlayerWeaponManager playerWeaponManager;
    private Animator playerAnimator;
    private PlayerVFX playerVFX;
    private PlayerSoundFXManager playerSFX;
    private PlayerStats playerStats;
    private PlayerCollection playerColllection;
    private PlayerStates playerStates;

    public float MaxHealth => playerStats.MaxHealth;
    public float Health => playerStats.CurrentHealth;
    public Action<float, float> OnHealthChanged { get; set; }
    public Action<float, float> onStaminaChanged;


    public bool CanTakeDamage
    {
        get { return !playerCombatManager.isInvulnerable; }
        private set { }
    }

    [SerializeField] public bool isDead = false;

    //private EventInstance lowStamInstance;

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        playerAnimator = GetComponent<Animator>();
        playerVFX = GetComponentInChildren<PlayerVFX>();
        playerSFX = GetComponent<PlayerSoundFXManager>();
        playerColllection = GetComponent<PlayerCollection>();

        playerStats = GetComponent<PlayerStats>();
        playerStates = GetComponent<PlayerStates>();
        Event_System.instance.OnLobbyLoaded += OnLobbyLoaded;
        Event_System.instance.OnLoadScenes += ReApplyStats;

        GetCharges(0); // Update the material of the cup at the start of the game with the initial healing charges
    }

    private void Update()
    {
        if (playerStats.CurrentHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        playerAnimator.SetBool("IsDead", isDead);

        playerVFX.SetVignetteIntensity(1 - (playerStats.currentStamina / playerStats.maxStamina));

        RuntimeManager.StudioSystem.setParameterByName("Stamina", playerStats.currentStamina / playerStats.maxStamina);
        RuntimeManager.StudioSystem.setParameterByName("Health", playerStats.CurrentHealth / playerStats.MaxHealth);
    }

    /// <summary>
    /// Applies incoming damage to the player,
    /// triggers effects and checks for death.
    /// </summary>
    /// <param name="damageInfo">Information about the damage dealt.</param>
    /// <param name="contactPoint">World position where damage occurred.</param>
    public void TakeDamage(DamageInfo damageInfo, Vector3 contactPoint)
    {
        if (CanTakeDamage && !isDead)
        {
            playerVFX.PlayBloodSplatter(contactPoint);
            playerSFX.PlayDamageGrunt();

            playerStats.CurrentHealth -= damageInfo.DamageAmount;
            NotifyHealthChanged();
            if (isDead)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        playerAnimator.SetBool("IsDead", true);
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
    }

    public void NotifyDeath()
    {
        Event_System.instance.OnPlayerDeath?.Invoke();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        playerStats.MaxHealth = newMaxHealth;

        playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth, 0, playerStats.MaxHealth);

        NotifyHealthChanged();
    }


    public void OnHeal()
    {
        bool attacking = playerStates.CurrentMoveState == MoveState.Attacking;
        bool dodging = playerStates.CurrentMoveState == MoveState.Dodging;

        if (!playerStates.IsHealing && !attacking && !dodging)
        {
            if (playerStats.currentHealingCharges >= playerStats.healingChargeCost && !isDead && Health < MaxHealth)
            {
                playerAnimator.SetTrigger("Drink");
            }
        }
    }

    public void Drink() // used in the animation event of the heal animation
    {
        Heal(playerStats.MaxHealth * playerStats.cupHealAmountPercentage);

        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.drinkingEvent, gameObject);

        playerStats.currentHealingCharges -= playerStats.healingChargeCost;
        CupCanvas.Instance.UpdateCup(playerStats.currentHealingCharges, playerStats.maxHealingCharges, playerStats.healingChargeCost);

        playerAnimator.ResetTrigger("Drink");
    }

    /// <summary>
    /// Restores health while respecting healing modifiers
    /// and maximum health limits.
    /// </summary>
    /// <param name="amount">Base healing amount.</param>
    public void Heal(float amount)
    {
        float totalHeal = amount * playerStats.currentHealModifier;
        playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth + totalHeal, 0, playerStats.MaxHealth);
        NotifyHealthChanged();
    }

    /// <summary>
    /// Adds healing charges and updates the healing cup UI.
    /// </summary>
    /// <param name="amount">Number of charges to add.</param>
    public void GetCharges(int amount)
    {
        playerStats.currentHealingCharges += amount;
        playerStats.currentHealingCharges = Mathf.Clamp(playerStats.currentHealingCharges, 0, playerStats.maxHealingCharges);
        CupCanvas.Instance.UpdateCup(playerStats.currentHealingCharges, playerStats.maxHealingCharges, playerStats.healingChargeCost);
    }

    public void NotifyStaminaChanged()
    {
        onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
    }

    private void OnDisable()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnConfirmCardSelection -= ApplyStatsFromCardSelection;
            Event_System.instance.OnSceneTransitionDone -= ReApplyStats;
        }
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnConfirmCardSelection += ApplyStatsFromCardSelection;
            Event_System.instance.OnSceneTransitionDone -= ReApplyStats;
        }
    }

    /// <summary>
    /// Rebuilds player statistics based on selected cards.
    /// Resets current health and stamina afterwards.
    /// </summary>
    /// <param name="cards">Selected card collection.</param>
    public void ApplyStatsFromCardSelection(List<CardData> cards)
    {

        InitializePlayerBaseStats();

        foreach (CardData card in cards)
        {
            if (card == null) continue;
            ApplyStatsInternally(card);
        }

        playerStats.CurrentHealth = MaxHealth;
        playerStats.currentStamina = playerStats.maxStamina;

        NotifyHealthChanged();
        NotifyStaminaChanged();
    }

    private void ApplySingleCard(CardData card)
    {
        if (card == null) return;

        ApplyStatsInternally(card);

        playerStats.CurrentHealth = MaxHealth;
        playerStats.currentStamina = playerStats.maxStamina;
    }

    /// <summary>
    /// Recalculates all player stats from the currently owned cards.
    /// Typically used after scene transitions.
    /// </summary>
    public void ReApplyStats()
    {
        List<CardData> templist = playerColllection.ReturnAllCards();
        InitializePlayerBaseStats();
        foreach (CardData card in templist)
        {
            ApplyStatsInternally(card);
        }
        NotifyHealthChanged();
        NotifyStaminaChanged();
    }

    private void OnLobbyLoaded()
    {
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.playerWakeUpEvent, gameObject);
        playerStats.CurrentHealth = 0;
        Heal(playerStats.MaxHealth);
        playerStats.currentHealingCharges = playerStats.startingCharges;
        CupCanvas.Instance.UpdateCup(playerStats.currentHealingCharges, playerStats.maxHealingCharges, playerStats.healingChargeCost);
        Event_System.instance.OnResetSouls.Invoke();
    }

    private void InitializePlayerBaseStats()
    {
        playerStats.MaxHealth = playerStats.baseHealth;
        playerStats.healthRegen = playerStats.baseStaminaRegeneration;
        playerStats.maxStamina = playerStats.baseStamina;
        playerStats.staminaRegen = playerStats.baseStaminaRegeneration;

        playerStats.currentLuck = playerStats.baseLuck;
        playerStats.currentCritChance = playerStats.baseCritChance;

        playerStats.currentWalkSpeedModifier = playerStats.baseWalkSpeedModifier;
        playerStats.currentSprintSpeedModifier = playerStats.baseSprintSpeedModifier;
        playerStats.currentDodgeSpeedModifier = playerStats.baseDodgeSpeedModifier;
        playerStats.currentDamageModifier = playerStats.baseDamageModifier;
        playerStats.currentHealModifier = playerStats.baseHealModifier;
        playerStats.currentKnockbackResistance = playerStats.baseKnockbackResistance;

        playerStats.currentWeaponSize = playerStats.baseWeaponSize;
        playerWeaponManager.currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;

        playerStats.currentActionSpeedModifier = playerStats.baseActionSpeed;

        foreach (WeaponStats weaponStats in GetComponentsInChildren<WeaponStats>(true))
        {
            weaponStats.DisableWeaponVFX();
        }

    }

    /// <summary>
    /// Applies stat modifiers from a single card.
    /// </summary>
    /// <param name="card">Card containing stat modifiers.</param>
    public void ApplyStatsInternally(CardData card)
    {
        playerStats.MaxHealth += card.healthModifier;
        playerStats.healthRegen += card.healRegeneraion;
        playerStats.maxStamina += card.staminaModifier;
        playerStats.staminaRegen += card.staminaRegeneraion;

        playerStats.currentLuck += card.luckModifier;
        playerStats.currentCritChance += card.critChance;


        playerStats.currentDodgeSpeedModifier += card.dodgeSpeedModifier;
        playerStats.currentDamageModifier += card.damageModifier;

        playerStats.currentHealModifier += card.healModifier;
        playerStats.currentKnockbackResistance += card.knockbackModifier;

        playerStats.currentWeaponSize += card.weaponSize;
        playerWeaponManager.currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;

        playerStats.currentActionSpeedModifier += card.actionSpeedModifier;
        playerAnimator.speed = playerStats.currentActionSpeedModifier;

        if (playerStats.currentWeaponSize.y > playerStats.maxWeaponSize.y || playerStats.currentWeaponSize.x > playerStats.maxWeaponSize.x)
        {
            playerStats.currentWeaponSize = new Vector3(playerStats.maxWeaponSize.x, playerStats.maxWeaponSize.y, playerStats.maxWeaponSize.z);
            playerWeaponManager.currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;
        }

        if (card.weaponVFX)
        {
            foreach (WeaponStats weaponStats in GetComponentsInChildren<WeaponStats>(true))
            {
                weaponStats.SetCardName(card.cardID);
                weaponStats.EnableWeaponVFX();
            }
        }
    }
}