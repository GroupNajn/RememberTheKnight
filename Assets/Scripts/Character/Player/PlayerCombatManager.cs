using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.AI;
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

    Animator playerAnimator;
    PlayerStats playerStats;
    PlayerManager playerManager;
    PlayerController playerController;

    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool canCombo = false;
    [SerializeField] public bool canCharge = false;
    [SerializeField] public bool fullyCharged = false;
    [SerializeField] public bool isAttackRotationSpeed = false;
    [SerializeField] public bool animationCanceleble { get; private set; } = true;
    public bool InCombat = false;
    [SerializeField] public StaminaAction currentAction;
    [SerializeField] public StaminaAction lastAttackAction;
    [SerializeField] EnemyCoordinator enemyCoordinator;


    public Dictionary<StaminaAction, float> StaminaCostBasedOnAction = new Dictionary<StaminaAction, float>()
    {
        {StaminaAction.Sprint, 5 },
        {StaminaAction.Dodge, 10 },
        {StaminaAction.lightAttack, 5 },
        {StaminaAction.heavyAttack, 25 }

    };

    [SerializeField] private float staminaRegenTime = 0f;

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

        playerAnimator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerManager = GetComponent<PlayerManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        enemyCoordinator = GameManager.Instance.GetComponentInChildren<EnemyCoordinator>();
        
    }
    public void ResetValues()
    {
        isInvulnerable = false;
        canCombo = false;
        canCharge = false;
        fullyCharged = false;
        isAttackRotationSpeed = false;
        animationCanceleble = true;
        //InCombat = false;
    }

    public bool CheckInCombat()
    {
        if (enemyCoordinator)
        {
            return enemyCoordinator.CombatEncounterInProgress;
        }
        return false;
    }

    public void SetStaminaState(StaminaAction action)
    {
        currentAction = action;

    }

    public void EnableInvulnerable()
    {
        isInvulnerable = true;

    }


    public void DisableInvulnerable()
    {
        isInvulnerable = false;
    }

    public void EnableCanCombo()
    {
        canCombo = true;
    }

    public void DisableCanCombo() // no longer used, but will cause errors if removed due to animation events
    {
        //canCombo = false;
        //animator.ResetTrigger("IsCombo");
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
        playerController.AttackCharged = true;
        playerAnimator.SetBool("IsCharged", true);

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
        if (lastAttackAction != StaminaAction.lightAttack)
            lastAttackAction = StaminaAction.lightAttack;
        currentAction = StaminaAction.lightAttack;

    }
    public void SetHeavyTrue()
    {
        if (lastAttackAction != StaminaAction.heavyAttack)
            lastAttackAction = StaminaAction.heavyAttack;
        currentAction = StaminaAction.heavyAttack;
    }

    public void DrainStamina()
    {
        if (playerStats.currentStamina > 0)
        {
            float staminaCost = StaminaCostBasedOnAction[currentAction];

            if (currentAction == StaminaAction.Sprint)
            {

                if (!CheckInCombat())  // If not in combat, sprinting doesn't drain stamina
                {
                    RegenerateStamina();
                    return;
                }
                staminaCost *= Time.deltaTime;
            }

            playerStats.currentStamina -= staminaCost;
            playerManager.onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);

            staminaRegenTime = 0;
        }
        else if (!CheckInCombat()) // If not in combat and stamina is depleted, regenerate stamina
        {
            RegenerateStamina();
        }
    }

    public void RegenerateStamina()
    {
        if (staminaRegenTime <= playerStats.staminaRegenDelay)
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
        // Debug.Log($"Gained {amount} stamina.");
        playerStats.currentStamina += amount;
        if (playerStats.currentStamina > playerStats.maxStamina)
            playerStats.currentStamina = playerStats.maxStamina;
        playerManager.onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
    }
}
