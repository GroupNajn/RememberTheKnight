using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
public enum StaminaAction
{
    Sprint,
    Dodge,
    heavyAttack,
    lightAttack
}
public class PlayerCombatManager : MonoBehaviour
{
    //Updated by Jonathan 2026-04-16
    public static PlayerCombatManager Instance { get; private set; }

    Animator animator;
    PlayerStats playerStats;
    PlayerManager playerManager;
    PlayerController playerController;

    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool canCombo = false;
    [SerializeField] public bool canCharge = false;
    [SerializeField] public bool fullyCharged = false;
    //[SerializeField] public bool Charging= false;
    [SerializeField] public bool isAttackRotationSpeed = false;
    [SerializeField] public bool animationCanceleble = true;

    [SerializeField] public StaminaAction currentAction;
    [SerializeField] public StaminaAction lastAttackAction;


    public Dictionary<StaminaAction, float> StaminaCostBasedOnAction = new Dictionary<StaminaAction, float>()
    {
        {StaminaAction.Sprint, 5 },
        {StaminaAction.Dodge, 20 },
        {StaminaAction.lightAttack, 5 },
        {StaminaAction.heavyAttack, 25 }

    };

    [SerializeField] private float staminaRegenDelay = 2f;
    [SerializeField] private float staminaRegenTime = 0f;
    private float lastStaminaUseTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager> ();
        playerController = GetComponent<PlayerController>();
    }

    public void SetStaminaState(StaminaAction action)
    {
        currentAction = action;
        //if (action == StaminaAction.lightAttack || action == StaminaAction.heavyAttack)
        //{
        //    lastAttackAction = action;
        //}
    }

    public void EnableInvulnerable()
    {
        isInvulnerable = true;

        // Debug.Log("Player is now invulnerable.");
    }


    public void DisableInvulnerable()
    {
        isInvulnerable = false;
        //  Debug.Log("Player is no longer invulnerable.");
    }

    public void EnableCanCombo()
    {
        canCombo = true;
    }

    public void DisableCanCombo()
    {
        canCombo = false;
    }

    public void EnableCanCharge()
    {
        canCharge = true;
    }
    public void DisableCanCharge()
    {
        canCharge = false;
    }

    public void FullyChargedTrue()
    {
        fullyCharged = true;
    }
    public void FullyChargedFalse()
    {
        fullyCharged = false;
    }


    public void SetAttackRotationSpeed()
    {
        isAttackRotationSpeed = true;
    }

    public void ResetAttackRotationSpeed()
    {
        isAttackRotationSpeed = false;
    }

    public void SetAnimationCancelebleFalse()
    {
        animationCanceleble = false;
    }
    public void SetAnimationCancelebleTrue()
    {
        animationCanceleble = true;   
    }
     
    public void SetHeavyFalse()
    {
        if (lastAttackAction == StaminaAction.heavyAttack)
            lastAttackAction = StaminaAction.lightAttack;
        currentAction = StaminaAction.lightAttack;

    }
    public void SetHeavyTrue()
    {
        if (lastAttackAction == StaminaAction.lightAttack)
            lastAttackAction = StaminaAction.heavyAttack;
        currentAction = StaminaAction.heavyAttack;
    }

    public void DrainStamina()
    {
        if (playerStats.currentStamina > 0)
        {
            float staminaCost = StaminaCostBasedOnAction[currentAction];

            if (currentAction == StaminaAction.Sprint)
                staminaCost *= Time.deltaTime;

            playerStats.currentStamina -= staminaCost;
            playerManager.onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);

            staminaRegenTime = 0;

            // Debug.Log($"stamina drain {staminaCost}");

        }
    }

    public void RegenerateStamina()
    {

        if (staminaRegenTime <= staminaRegenDelay)
        {
            staminaRegenTime += Time.deltaTime;
            return;
        }

        if (playerStats.currentStamina < playerStats.maxStamina)
        {
            playerStats.currentStamina += playerStats.staminaRegenRate * Time.deltaTime;

            if (playerStats.currentStamina > playerStats.maxStamina)
                playerStats.currentStamina = playerStats.maxStamina;

            playerManager.onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
        }
    }

    public void GainStamina(float amount)
    {
        Debug.Log($"Gained {amount} stamina.");
        playerStats.currentStamina += amount;
        if (playerStats.currentStamina > playerStats.maxStamina)
            playerStats.currentStamina = playerStats.maxStamina;
        playerManager.onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
    }
}
