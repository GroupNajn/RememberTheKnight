using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{ 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerStates playerStates; 
    PlayerStats playerStats;
    public enum StaminaAction 
    { Sprinting, Dodging, heavyAttacking, lightAttacking } 
    [Header("Stamina Costs")] 
    [SerializeField] float sprintDrainPerSecond = 0.5f;
    [SerializeField] float dodgeCost = 20f; 
    [SerializeField] float heavyAttackCost = 30f; 
    [SerializeField] float lightAttackCost = 5f; 
    private float staminaRegenDelay = 1f; 
    private float lastStaminaUseTime; 
    
    void Start() 
    { 
        playerStates = GetComponent<PlayerStates>(); 
        playerStats = GetComponent<PlayerStats>(); 
    } // Update is called once per frame
    
    void Update() 
    {
        if (playerStates.CurrentMoveState == MoveState.Sprinting) 
        { UseStamina(StaminaAction.Sprinting); } else { RegenerateStamina(); }
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
        { UseStamina(StaminaAction.Dodging); } 
    } 
    
    public bool UseStamina(StaminaAction action) 
    { 
        float cost = GetCost(action); 
        if (playerStats.currentStamina >= cost) 
        { 
            playerStats.currentStamina -= cost;
            playerStats.onStaminaChange?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
            lastStaminaUseTime = Time.time;
            return true; 
        } 
        return false; 
    } 
    public float GetCost(StaminaAction action) 
    { 
        switch(action) 
        { 
            case StaminaAction.Sprinting: return sprintDrainPerSecond * Time.deltaTime;
            case StaminaAction.Dodging: return dodgeCost;
            case StaminaAction.heavyAttacking: return heavyAttackCost;
            case StaminaAction.lightAttacking: return lightAttackCost;
            default: return 0f; 
        };
    }
    
    public void RegenerateStamina()
    {
        if (Time.time < lastStaminaUseTime + staminaRegenDelay) 
        { return; } 
        if (playerStats.currentStamina < playerStats.maxStamina) 
        { 
            playerStats.currentStamina += playerStats.staminaRegenRate * Time.deltaTime;
            playerStats.currentStamina = Mathf.Clamp(playerStats.currentStamina, 0, playerStats.maxStamina);
            playerStats.onStaminaChange?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
        } 
    }

    public bool CanPerform(StaminaAction action)
    {
        float cost = GetCost(action);

        if (action == StaminaAction.Sprinting)
            return playerStats.currentStamina > 0f;

        return playerStats.currentStamina >= cost;
    }
}
 