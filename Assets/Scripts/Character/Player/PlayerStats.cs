using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    // Made by Lukas 2026-03-14
    // Updated by Lukas and Jonatan and Wilmer 2026-03-16



    private PlayerCombatManager playerCombatManager;
    private Animator playerAnimator;


    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float Health { get; set; }

    [Header("Stats")]
    public float walkSpeedMultiplier = 0f;
    public float sprintSpeedMultiplier = 0f;
    public float gravity = 25f;
    public float normalRotationSpeed = 10f;
    public float attackRotationSpeed = 5f;
    public float dodgeSpeedMultiplier = 0f;
    public float dodgeCoolDown = 1f;
    public float dodgeDuration = 0.2f;

    public float knockbackResistance = 5f;

    // [HideInInspector]

    [HideInInspector]
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
        Health = MaxHealth;
    }

    private void Update()
    {
        if (Health <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;        
        }
        playerAnimator.SetBool("IsDead", isDead);
    }

    public void TakeDamage(float damage)
    {
        if (CanTakeDamage && !isDead)
        {
            Health -= damage;
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
}