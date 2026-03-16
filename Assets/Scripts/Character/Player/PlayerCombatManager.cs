using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager Instance { get; private set; }

    [SerializeField] Animator animator;

    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool canCombo = false;
    [SerializeField] public bool isAttackRotationSpeed = false;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
}
