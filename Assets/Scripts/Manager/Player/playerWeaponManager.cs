using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : CharacterWeaponManager
{
    public GameObject CurrentRightHandWeapon => currentRightHandWeapon;

    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager playerCombatManager;
    PlayerStats playerStats;

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
    }
    private void OnHolster(InputValue action)
    {
        Holsterd = !Holsterd;
        HolsterCheck();
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
