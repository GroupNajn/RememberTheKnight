using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    // Made by Lukas 2026-03-14
    // Updated by Lukas and Jonatan 2026-03-16

    [field: SerializeField] public int MaxHealth { get; private set; }

    private PlayerCombatManager playerCombatManager;
    private Animator playerAnimator;
    // [HideInInspector]
    [field: SerializeField] public int Health { get; set; }
    [HideInInspector]
    public bool CanTakeDamage
    {
        get { return !playerCombatManager.isInvulnerable; }
        private set { }
    }

    [SerializeField] private bool isDead = false;

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

    public void TakeDamage(int damage)
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