using UnityEngine;
using System.Collections.Generic;
public enum StaminaAction
{
    Sprint,
    Dodge,
    heavyAttack,
    lightAttack
}
public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager Instance { get; private set; }

    Animator animator;
    PlayerStats playerStats;

    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool canCombo = false;
    [SerializeField] public bool isAttackRotationSpeed = false;
    [SerializeField] public bool animationCanceleble = true;

    [SerializeField] public StaminaAction currentAction;

    public Dictionary<StaminaAction, float> StaminaCostBasedOnAction = new Dictionary<StaminaAction, float>()
    {
        {StaminaAction.Sprint, 5 },
        {StaminaAction.Dodge, 20 },
        {StaminaAction.lightAttack, 5 },
        {StaminaAction.heavyAttack, 15 }

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


    public void DrainStamina()
    {
        if (playerStats.currentStamina > 0)
        {
            float staminaCost = StaminaCostBasedOnAction[currentAction];

            if(currentAction == StaminaAction.Sprint)
                staminaCost *= Time.deltaTime;

            playerStats.currentStamina -= staminaCost;
            playerStats.onStaminaChange?.Invoke(playerStats.currentStamina, playerStats.maxStamina);

            staminaRegenTime = 0;
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

            if(playerStats.currentStamina > playerStats.maxStamina)
                playerStats.currentStamina = playerStats.maxStamina;

            playerStats.onStaminaChange?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
        }
    }
}
