using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarCanvas : MonoBehaviour
{
    [SerializeField] float displayDuration = 2f; // Duration to show the health bar after taking damage

    EnemyDamage enemyDamage;
    Slider healthBar;
    bool isOnCooldown = false;

    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();

        enemyDamage = GetComponentInParent<EnemyDamage>();
        if (enemyDamage == null)
        {
            Debug.Log("EnemyDamage component not found in parent!");
            return;
        }
        enemyDamage.OnHealthChanged += ShowHealthBarForDuration;

        Event_System.instance.OnEnemyDamage += ShowHealthBarForDuration;

        HideHealthBar(); // Start with the canvas hidden immediately
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void ShowHealthBar() // Used for locking on target
    {
        healthBar.gameObject.SetActive(true);
        StopCoroutine(HideHealthBarAfterDelay()); // Stop any existing hide coroutine to keep the canvas visible
    }

    public void ShowHealthBarForDuration(Transform target, float damage) // Used for showing the canvas when taking damage
    {
        if (target.root != transform.root || healthBar == null) 
            return; // Ensure the event is for this enemy
        healthBar.gameObject.SetActive(true);
        StopCoroutine(HideHealthBarAfterDelay()); // Stop any existing hide coroutine to keep the canvas visible
    }

    public void ShowHealthBarForDuration(float current = 0, float max = 0) // Parameters are required to match the OnHealthChanged signature, but we don't use them here
    {
        healthBar.gameObject.SetActive(true);
        StartCoroutine(HideHealthBarAfterDelay());
    }

    public void HideHealthBar()
    {
        if (healthBar == null || isOnCooldown)
            return;

        healthBar.gameObject.SetActive(false);
    }

    IEnumerator HideHealthBarAfterDelay()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(displayDuration);
        isOnCooldown = false;
        HideHealthBar();
    }
}