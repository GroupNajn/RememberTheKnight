using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] bool isInvulnerable = false;
    [SerializeField] bool canCombo = false;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        PerformCombo();
    }
    public void EnableInvulnerable()
    {
        isInvulnerable = true;

        Debug.Log("Player is now invulnerable.");
    }

    public void DisableInvulnerable()
    {
        isInvulnerable = false;
        Debug.Log("Player is no longer invulnerable.");
    }

    public void EnableCanCombo()
    {
        canCombo = true;
    }

    public void DisableCanCombo()
    {
        canCombo = false;
    }

    private void PerformCombo()
    {
        if (canCombo)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                animator.SetBool("IsCombo", true);
                canCombo = false;
            }
        }
        else
        {
            animator.SetBool("IsCombo", false);
        }
    }


}
