using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerWeaponManager : CharacterWeaponManager
{
    public GameObject CurrentRightHandWeapon => currentRightHandWeapon;
    public System.Action<WeaponData> OnWeaponChanged;

    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager playerCombatManager;
    PlayerStats playerStats;

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

        playerStats.baseWeaponSize = currentRightHandWeapon.transform.localScale;
    }
    public void OnHolster(InputValue action)
    {
        holsterd = !holsterd;

        HolsterCheck();
    }

    public void SwitchWeapon()
    {
        if (Weapons.Count == 0) return;

        if (Holsterd)
        {
            OnHolster(null);
        }

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
        playerAnimator.speed = stats.WeaponData.AnimatorSpeed;

        equippedWeapon = stats.WeaponData;

        // sätter vapen storleken till base när man byter vapen
        //playerStats.currentWeaponSize = playerStats.baseWeaponSize;
        //Debug.Log($"base är {playerStats.currentWeaponSize}");
        // sätter sedan vapnet till den sizen spelaren stats säger
        currentRightHandWeapon.transform.localScale = playerStats.currentWeaponSize;

        OnWeaponChanged?.Invoke(equippedWeapon);

        // gå vidare till nästa för nästa interaction
        currentWeaponIndex = (currentWeaponIndex + 1) % Weapons.Count;
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

    public override float CalculateFinalDamage(WeaponData weaponData)
    {
        // finalDamage = weaponData.base + weapondaata.charged + damgemodifier 
        if (playerController.AttackCharged)
            finalDamage = (damageAmount + chargedDamageBonus) * playerStats.currentDamageModifier;
        else
            finalDamage = damageAmount * playerStats.currentDamageModifier;

        return finalDamage;
    }
}
