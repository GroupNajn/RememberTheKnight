using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDamageable
{
    private PlayerCombatManager playerCombatManager;
    private Animator playerAnimator;
    private PlayerVFX playerVFX;
    private PlayerStats playerStats;

    public float MaxHealth => playerStats.MaxHealth;
    public float Health  => playerStats.Health;
    public Action<float, float> OnHealthChanged { get; set; }
    public Action<float, float> onStaminaChanged;


    public bool CanTakeDamage
    {
        get { return !playerCombatManager.isInvulnerable; }
        private set { }
    }


    [SerializeField] public bool isDead = false;

    private void Start()
    {
        playerCombatManager = PlayerCombatManager.Instance;
        playerAnimator = GetComponent<Animator>();
        playerVFX = GetComponentInChildren<PlayerVFX>();

        playerStats = GetComponent<PlayerStats>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (playerStats.Health <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
        playerAnimator.SetBool("IsDead", isDead);

    }

    public void TakeDamage(float damage, Vector3 contactPoint)
    {
        if (CanTakeDamage && !isDead)
        {
            playerVFX.PlayBloodSplatter(contactPoint);

            playerStats.Health -= damage;
            NotifyHealthChanged();
            if (isDead)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Debug.Log("DIE!");
        playerAnimator.SetBool("IsDead", true);
    }

    private void NotifyHealthChanged()
    {

        Debug.Log("EVENT TRIGGERED: " + playerStats.Health);
        OnHealthChanged?.Invoke(playerStats.Health, playerStats.MaxHealth);
    }

    public void NotifyDeath()
    {
        Debug.Log("EVENT TRIGGERED: Player Died");
        Event_System.instance.OnPlayerDeath?.Invoke();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        playerStats.MaxHealth = newMaxHealth;

        playerStats.Health = Mathf.Clamp(playerStats.Health, 0, playerStats.MaxHealth);

        NotifyHealthChanged();
    }

    public void Heal(float amount)
    {
        float totalHeal = amount * playerStats.healMultiplier;
        playerStats.Health = Mathf.Clamp(playerStats.Health + totalHeal, 0, playerStats.MaxHealth);
        NotifyHealthChanged();
    }

    public void UpdateMaxStamina(float newMaxStamina)
    {
        playerStats.maxStamina = newMaxStamina;

        onStaminaChanged?.Invoke(playerStats.currentStamina, playerStats.maxStamina);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneData.Instance[2]) // Heal to max health after loading lobby
        {
            Heal(playerStats.MaxHealth);
        }
    }
}
