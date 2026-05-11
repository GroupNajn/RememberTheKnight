using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDamageable
{
    private PlayerCombatManager playerCombatManager;
    private PlayerWeaponManager playerWeaponManager;
    private Animator playerAnimator;
    private PlayerVFX playerVFX;
    private PlayerSoundFXManager playerSFX;
    private PlayerStats playerStats;
    private PlayerCollection playerColllection;

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

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        playerAnimator = GetComponent<Animator>();
        playerVFX = GetComponentInChildren<PlayerVFX>();
        playerSFX = GetComponent<PlayerSoundFXManager>();
        playerColllection = GetComponent<PlayerCollection>();

        playerStats = GetComponent<PlayerStats>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Event_System.instance.OnStatsApplied += ApplyStatsFromCardSelection;
        //Event_System.instance.OnCardPickedUp += ReApplyStats;

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

    }

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
                // MAYBE PLAY DEATH SOUND FX
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

        //Debug.Log("EVENT TRIGGERED: " + playerStats.Health);
        OnHealthChanged?.Invoke(playerStats.CurrentHealth, playerStats.MaxHealth);
    }

    public void NotifyDeath()
    {
        //Debug.Log("EVENT TRIGGERED: Player Died");
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
        Debug.Log("Attempting to heal. Current Charges: " + playerStats.currentHealingCharges);
        if (playerStats.currentHealingCharges >= playerStats.healingChargeCost)
        {
            Heal(playerStats.cupHealAmount);
            playerStats.currentHealingCharges -= playerStats.healingChargeCost;
            CupCanvas.Instance.UpdateCup(playerStats.currentHealingCharges, playerStats.maxHealingCharges, playerStats.healingChargeCost);
        }
    }

    public void Heal(float amount)
    {
        float totalHeal = amount * playerStats.currentHealModifier;
        playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth + totalHeal, 0, playerStats.MaxHealth);
        NotifyHealthChanged();
    }

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
        }
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnConfirmCardSelection += ApplyStatsFromCardSelection;
        }
    }

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
        if(card == null) return;
       
        ApplyStatsInternally(card);

        playerStats.CurrentHealth = MaxHealth;
        playerStats.currentStamina = playerStats.maxStamina;
    }
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneData.Instance[2]) // Heal to max health after loading lobby
        {
            Heal(playerStats.MaxHealth);
            playerStats.currentHealingCharges = playerStats.startingCharges;
            CupCanvas.Instance.UpdateCup(playerStats.currentHealingCharges, playerStats.maxHealingCharges, playerStats.healingChargeCost);
            Event_System.instance.OnResetSouls.Invoke();
        }
    }

    private void InitializePlayerBaseStats()
    {
        playerStats.MaxHealth = playerStats.baseHealth;
        playerStats.maxStamina = playerStats.baseStamina;

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
    }

    public void ApplyStatsInternally(CardData card)
    {
        playerStats.MaxHealth += card.healthModifier;
        playerStats.maxStamina += card.staminaModifier;

        playerStats.currentLuck += card.luckModifier;
        playerStats.currentCritChance += card.critChance;

        playerStats.currentWalkSpeedModifier += card.walkSpeedModifier;
        playerStats.currentSprintSpeedModifier += card.sprintSpeedModifier;
        playerStats.currentDodgeSpeedModifier += card.dodgeSpeedModifier;
        playerStats.currentDamageModifier += card.damageModifier;


        playerStats.currentHealModifier += card.healModifier;
        playerStats.currentKnockbackResistance += card.knockbackModifier;

        playerStats.currentWeaponSize += card.weaponSize;
        playerWeaponManager.currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;
    }
}