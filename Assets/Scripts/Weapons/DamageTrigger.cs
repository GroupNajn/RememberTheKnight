using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponStats))]
public class DamageTrigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Anton A 2026-03-16
    // Updated by Jonathan  2026-04-16

    WeaponData weaponData;
    //float damageAmount
    //{
    //    get
    //    {
    //        if (combatManager.lastAttackAction == StaminaAction.lightAttack)
    //            return weaponData.BaseDamage;

    //        else return weaponData.HeavyDamage;
    //    }
    //    set { }
    //}
    //float chargedDamageBonus
    //{
    //    get
    //    {
    //        if (combatManager.lastAttackAction == StaminaAction.lightAttack)
    //            return weaponData.ChargedDamageBonus;

    //        else return weaponData.HeavyChargedDamage;
    //    }
    //    set { }
    //}

    GameObject player;
    [SerializeField] PlayerController playerController;
    PlayerCombatManager combatManager;
    PlayerLocomotion playerLocomotion;
    Animator playerAnimator;
    PlayerWeaponManager playerWeaponManager;
    EnemyWeaponManager enemyWeaponManager;


    HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Start()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        combatManager = player.GetComponent<PlayerCombatManager>();
        playerLocomotion = player.GetComponent<PlayerLocomotion>();
        playerWeaponManager = player.GetComponent<PlayerWeaponManager>();
        playerAnimator = player.GetComponent<Animator>();
        if (weaponData != null)
        {
            //damageAmount = weaponData.BaseDamage;
            //chargedDamageBonus = weaponData.ChargedDamageBonus;
        }
        else
        {
            // NOT NEEDED ANY MORE 
            //damageAmount = 999;
            //chargedDamageBonus = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStates check = this.gameObject.GetComponentInParent<PlayerStates>();

        if (check == null)
        {
            enemyWeaponManager = this.gameObject.GetComponentInParent<EnemyWeaponManager>();
        }

        if (check && other.gameObject == player)// prevent damaging self with own weapon
        {
            return;
        }

        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null && damagedObjects.Add(damageable))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            if (playerController != null) // stamina gain if hit with a charged attack
            {
                if (playerController.AttackCharged)
                {
                    combatManager.GainStamina(30);
                }
                damageable.TakeDamage(playerWeaponManager.CalculateFinalDamage(playerWeaponManager.currentActiveWeaponData), contactPoint);
                return;
            }

            damageable.TakeDamage(enemyWeaponManager.CalculateFinalDamage(enemyWeaponManager.currentActiveWeaponData), contactPoint);
        }
    }

    public void ResetDamage()
    {
        damagedObjects.Clear();
    }
}