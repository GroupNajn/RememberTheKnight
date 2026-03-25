using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager Instance { get; private set; }

    Animator animator;

    [SerializeField] public bool isInvulnerable = false;
    [SerializeField] public bool canCombo = false;
    [SerializeField] public bool isAttackRotationSpeed = false;
    [SerializeField] public bool animationCanceleble = true;

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
}
