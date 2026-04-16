using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WeaponStats))]
public class DamageTrigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Anton A 2026-03-16
    // Updated by Jonathan  2026-04-16

    WeaponData weaponData;
    float damageAmount
    {
        get
        {
            if (combatManager.lastAttackAction == StaminaAction.lightAttack)
                return weaponData.BaseDamage;

            else return weaponData.HeavyDamage;
        }
        set { }
    }
    float chargedDamageBonus
    {
        get
        {
            if (combatManager.lastAttackAction == StaminaAction.heavyAttack)
                return weaponData.ChargedDamageBonus;

            else return weaponData.HeavyChargedDamage;
        }
        set { }
    }
    //float damageAmount
    //{
    //    get
    //    {
    //        if (playerLocomotion.AttackPressed)
    //            return weaponData.BaseDamage;

    //        else return weaponData.HeavyDamage;
    //    }
    //    set { }
    //}
    //float chargedDamageBonus
    //{
    //    get
    //    {
    //        if (playerLocomotion.AttackPressed)
    //            return weaponData.ChargedDamageBonus;

    //        else return weaponData.HeavyChargedDamage;
    //    }
    //    set { }
    //}
    //float chargedDamageBonus;
    GameObject player;
    PlayerController playerController;
    PlayerCombatManager combatManager;
    PlayerLocomotion playerLocomotion;


    HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Start()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        combatManager = player.GetComponent<PlayerCombatManager>();
        playerLocomotion = player.GetComponent<PlayerLocomotion>();
        if (weaponData != null)
        {
            //damageAmount = weaponData.BaseDamage;
            //chargedDamageBonus = weaponData.ChargedDamageBonus;
        }
        else
        {
            damageAmount = 999;
            chargedDamageBonus = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStates check = this.gameObject.GetComponentInParent<PlayerStates>();
        if (check && other.gameObject == player)// prevent damaging self with own weapon
        {
            Debug.Log($"Prevented damage from: {check.gameObject}, to: {other.gameObject}");
            return;
        }

        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null && damagedObjects.Add(damageable))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            if (other.gameObject != player) // stamina gain if hit with a charged attack
            {
                if (playerController.AttackCharged)
                {
                    combatManager.GainStamina(30);
                    playerController.AttackCharged = false;
                    damageable.TakeDamage(damageAmount + chargedDamageBonus, contactPoint);
                    return;
                }

            }

            damageable.TakeDamage(damageAmount, contactPoint);
        }
    }

    public void ResetDamage()
    {
        damagedObjects.Clear();
        Debug.Log("Damage reset, ready to damage new targets.");
    }
}