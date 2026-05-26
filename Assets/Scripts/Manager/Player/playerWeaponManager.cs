using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : CharacterWeaponManager
{
    public GameObject CurrentRightHandWeapon => currentRightHandWeapon;
    public System.Action<WeaponData> OnWeaponChanged;

    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager playerCombatManager;
    PlayerStats playerStats;
    PlayerManager playerManager;

    [SerializeField] public List<GameObject> Weapons;
    int currentWeaponIndex = 0;

    float damageAmount
    {
        get
        {
            if (playerCombatManager.lastAttackAction == StaminaAction.lightAttack)
                return currentActiveWeaponData.LightDamage;

            else return currentActiveWeaponData.HeavyDamage;
        }
        set { }
    }
    float chargedDamageBonus
    {
        get
        {
            if (playerCombatManager.lastAttackAction == StaminaAction.lightAttack)
                return currentActiveWeaponData.LightChargedDamageBonus;

            else return currentActiveWeaponData.HeavyChargedDamage;
        }
        set { }
    }

    public override void Start()
    {
        base.Start();

        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();

        playerStats.baseWeaponSize = currentRightHandWeapon.transform.localScale;

        OnHolster(null);
    }
    public void OnHolster(InputValue action)
    {
        holsterd = !holsterd;

        HolsterCheck();
        playerManager.ReApplyStats();
        OnWeaponChanged?.Invoke(currentActiveWeaponData);
    }

    public override void HolsterCheck()
    {
        base.HolsterCheck();

        playerAnimator.runtimeAnimatorController = currentActiveWeaponData.WeaponAnimator;
    }

    public void SwitchWeapon()
    {
        if (Weapons.Count == 0) return;

        if (Holsterd)
        {
            OnHolster(null);
        }

        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;

        currentWeaponIndex %= Weapons.Count;

        // Stäng av alla
        foreach (var weapon in Weapons)
        {
            weapon.SetActive(false);
        }

        GameObject currentWeapon = Weapons[currentWeaponIndex];
        currentWeapon.SetActive(true);

        var stats = currentWeapon.GetComponent<WeaponStats>();

        currentRightHandWeapon = currentWeapon;
        currentActiveWeaponData = stats.WeaponData;
        currentRightWeaponData = stats.WeaponData;
        rightDamageTrigger = currentWeapon.GetComponent<DamageTrigger>();

        playerAnimator.runtimeAnimatorController = stats.WeaponData.WeaponAnimator;
        playerAnimator.speed = stats.WeaponData.actionSpeed;

        equippedWeapon = stats.WeaponData;

        // sätter vapen storleken till base när man byter vapen
        //playerStats.currentWeaponSize = playerStats.baseWeaponSize;
        //Debug.Log($"base är {playerStats.currentWeaponSize}");
        // sätter sedan vapnet till den sizen spelaren stats säger
        currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;

        playerStats.baseActionSpeed = stats.WeaponData.actionSpeed;

        OnWeaponChanged?.Invoke(equippedWeapon);

        playerManager.ReApplyStats();

        // gå vidare till nästa för nästa interaction
    }

    public override void DeactivateRightDamageCollider()
    {
        base.DeactivateRightDamageCollider();

        if (currentRightHandWeapon != null)
        {
            playerController.AttackCharged = false;
            playerAnimator.SetBool("IsCharged", false);
        }
    }

    public override void DeactivateLeftDamageCollider()
    {
        base.DeactivateLeftDamageCollider();

        if (currentLeftHandWeapon != null)
        {
            playerController.AttackCharged = false;
            playerAnimator.SetBool("IsCharged", false);

        }
    }

    public new DamageInfo CalculateFinalDamage(WeaponData weaponData)
    {
        // finalDamage = weaponData.base + weapondaata.charged + damgemodifier 
        DamageInfo damageInfo = new DamageInfo();
        float critRoll = Random.Range(0f, 1f);
        if (critRoll <= playerStats.currentCritChance / 100)
        {
            damageInfo.SetIsCrit(true);
        }

        if (playerController.AttackCharged && !damageInfo.IsCrit)
            damageInfo.SetDamageAmount((damageAmount + chargedDamageBonus) * playerStats.currentDamageModifier);

        else if (playerController.AttackCharged && damageInfo.IsCrit)
            damageInfo.SetDamageAmount(((damageAmount + chargedDamageBonus) * playerStats.currentDamageModifier) * 2);

        else if (damageInfo.IsCrit)
            damageInfo.SetDamageAmount((damageAmount * playerStats.currentDamageModifier) * 2);

        else
            damageInfo.SetDamageAmount(damageAmount * playerStats.currentDamageModifier);

        return damageInfo;
    }
}