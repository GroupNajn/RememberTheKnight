using FMODUnity;
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
    Collider selfCollider;


    HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Awake()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        combatManager = player.GetComponent<PlayerCombatManager>();
        playerLocomotion = player.GetComponent<PlayerLocomotion>();
        playerWeaponManager = player.GetComponent<PlayerWeaponManager>();
        playerAnimator = player.GetComponent<Animator>();


    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStates check = this.gameObject.GetComponentInParent<PlayerStates>();

        if (gameObject.TryGetComponent<Projectile>(out Projectile projectile))
        {
            enemyWeaponManager = projectile.enemyWeaponManager;
        }
        if (check == null && enemyWeaponManager == null)// om inte spelare
        {
            enemyWeaponManager = this.gameObject.GetComponentInParent<EnemyWeaponManager>();
        }

        if (check && (other.gameObject == player))// prevent player from damaging self with own weapon
        {
            return;
        }

        if (transform.IsChildOf(other.transform))
        {
            return;
        }

        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null && damagedObjects.Add(damageable))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            if (playerController != null && check != null) // if player
            {
                if (playerController.AttackCharged)
                {
                    combatManager.GainStamina(30);
                    RuntimeManager.PlayOneShotAttached(playerController.gameObject.GetComponent<PlayerSoundFXManager>().FullyChargedEvent, playerController.gameObject);
                }
                damageable.TakeDamage(playerWeaponManager.CalculateFinalDamage(playerWeaponManager.currentActiveWeaponData), contactPoint);
                RuntimeManager.PlayOneShotAttached(playerWeaponManager.currentActiveWeaponData.HitEvent, other.gameObject);
                return;
            }
            damageable.TakeDamage(enemyWeaponManager.CalculateFinalDamage(weaponData), contactPoint);
            RuntimeManager.PlayOneShotAttached(enemyWeaponManager.currentActiveWeaponData.HitEvent, other.gameObject);
        }
    }

    public void ResetDamage()
    {
        //Debug.Log("Damage reset for " + gameObject.name);
        damagedObjects.Clear();
    }
}