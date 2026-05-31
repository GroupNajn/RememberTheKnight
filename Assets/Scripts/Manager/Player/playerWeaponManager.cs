using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerWeaponManager : CharacterWeaponManager
{
    public GameObject CurrentRightHandWeapon => currentRightHandWeapon;
    public System.Action<WeaponData> OnWeaponChanged;

    PlayerController playerController;
    Animator playerAnimator;
    PlayerCombatManager playerCombatManager;
    PlayerStats playerStats;
    PlayerManager playerManager;
    PlayerStates playerStates;

    [SerializeField] public List<GameObject> Weapons;
    [SerializeField] public List<GameObject> HolsterdWeapons;
    int currentWeaponIndex = 0;
    int layerIndex;
    AnimatorStateInfo currentState;


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
        playerStates = GetComponent<PlayerStates>();


        playerStats.baseWeaponSize = currentRightHandWeapon.transform.localScale;

        HolsterEvent();

        layerIndex = playerAnimator.GetLayerIndex("Holster");


        Event_System.instance.OnLoadScenes += OnLoadScenes;
    }

    private void OnLoadScenes()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == SceneData.Instance[1] || sceneName == SceneData.Instance[1])
        {
            // Make sure player is always holstered when entering lobby and character select screen 
            if (!holsterd)
            {
                HolsterEvent();
            }
        }
    }

    public void OnHolster(InputValue action)
    {
        if (!playerStates.InActionState())
        {
            RuntimeManager.PlayOneShotAttached(holsterd? WorldSoundFXManager.instance.unHolsterEvent : WorldSoundFXManager.instance.holsterEvent, gameObject);
            playerAnimator.SetTrigger("Holster");
        }
    }

    public void HolsterEvent() // Called from animation event
    {
        holsterd = !holsterd;

        HolsterCheck();
        playerManager.ReApplyStats();
        OnWeaponChanged?.Invoke(currentActiveWeaponData);

        if (holsterd)
        {
            HolsterdWeapons[currentWeaponIndex].SetActive(true);
        }
        else
        {
            HolsterdWeapons[currentWeaponIndex].SetActive(false);
        }


    }

    public override void HolsterCheck()
    {
        base.HolsterCheck();

        playerAnimator.runtimeAnimatorController = currentActiveWeaponData.WeaponAnimator;
        playerAnimator.speed = currentActiveWeaponData.actionSpeed;

    }

    public override void Update()
    {
        base.Update();


        currentState = playerAnimator.GetCurrentAnimatorStateInfo(layerIndex); // Holster layer
        bool inTransition = playerAnimator.IsInTransition(layerIndex);

        if (currentState.IsTag("Holstering") || inTransition) // check Holstering tag
        {
            playerStates.SetIsHolstering(true);
        }
        else
        {
            playerStates.SetIsHolstering(false);
        }
    }

    public void SwitchWeapon()
    {
        if (Weapons.Count == 0) return;

        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.weaponSwitchEvent);
        if (Holsterd)
        {
            HolsterEvent();
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