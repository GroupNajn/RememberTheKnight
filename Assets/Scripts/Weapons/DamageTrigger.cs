using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WeaponStats))]
public class DamageTrigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Anton A 2026-03-16

    WeaponData weaponData;
    float damageAmount;
    GameObject player;
    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager combatManager;

    private int chargedHash = Animator.StringToHash("ChargedAttack");



    HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Start()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        combatManager = player.GetComponent<PlayerCombatManager>();
        if (weaponData != null)
        {
            damageAmount = weaponData.BaseDamage;
        }
        else
        {
            damageAmount = 999;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null && damagedObjects.Add(damageable))
        {

            if (other.gameObject != player && playerController.AttackCharged) // stamina gain if hit with a charged attack
            {
                combatManager.GainStamina(50);
                playerController.AttackCharged = false;
            }

            Vector3 contactPoint = other.ClosestPoint(transform.position);

            damageable.TakeDamage(damageAmount, contactPoint);
        }
    }

    public void ResetDamage()
    {
        damagedObjects.Clear();
        Debug.Log("Damage reset, ready to damage new targets.");
    }
}