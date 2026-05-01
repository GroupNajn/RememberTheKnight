using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerWeaponManager : CharacterWeaponManager
{
    public GameObject CurrentRightHandWeapon => currentRightHandWeapon;

    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager playerCombatManager;
    PlayerStats playerStats;

    [SerializeField] GameObject unarmedWeapon;
    Image weaponIconImage;

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

        weaponIconImage = GameObject.FindGameObjectWithTag("WeaponIconImage")?.GetComponent<Image>();
    }
    public void OnHolster(InputValue action)
    {
        holsterd = !holsterd;

        if (holsterd)
        {
            if (!weaponIconImage)
            {
                weaponIconImage = GameObject.FindGameObjectWithTag("WeaponIconImage").GetComponent<Image>();
            }

            weaponIconImage.sprite = unarmedWeapon.GetComponent<WeaponStats>().WeaponData.WeaponIcon;
        }
        else
        {
            if (!weaponIconImage)
            {
                weaponIconImage = GameObject.FindGameObjectWithTag("WeaponIconImage").GetComponent<Image>();
            }

            weaponIconImage.sprite = currentRightHandWeapon.GetComponent<WeaponStats>().WeaponData.WeaponIcon;
        }

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
