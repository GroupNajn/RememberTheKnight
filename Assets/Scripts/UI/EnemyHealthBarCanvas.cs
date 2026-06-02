using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls visibility of enemy health bars.
/// Health bars appear when damage is taken or when locked on,
/// and automatically hide after a delay.
/// </summary>
public class EnemyHealthBarCanvas : MonoBehaviour
{
    // Edited by Michaëla 2026-05-06
    // Edited by Lukas 2026-05-20
    [SerializeField] float displayDuration = 2f; // Duration to show the health bar after taking damage

    EnemyDamage enemyDamage;
    Slider healthBar;
    bool isOnCooldown = false;
    [SerializeField] bool allowCoolDown = false;
    Coroutine hideCoroutine;

    void Awake() //Gets references to the healthbar and enemydamage component
    {
        healthBar = GetComponentInChildren<Slider>();
        enemyDamage = GetComponentInParent<EnemyDamage>();
    }
    private void Start() // Subscribes to the OnHealthChanged event and the OnEnemyDamage event, and hides the health bar at the start
    {
        enemyDamage.OnHealthChanged += ShowHealthBarForDuration;
        Event_System.instance.OnEnemyDamage += ShowHealthBarForDuration;

        HideHealthBar();
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// Displays the health bar indefinitely.
    /// Used when the enemy is currently targeted.
    /// </summary>
    public void ShowHealthBar() // Used for locking on target
    {
        healthBar.gameObject.SetActive(true);
        allowCoolDown = false;
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);
    }

    /// <summary>
    /// Displays the health bar temporarily after damage is taken.
    /// </summary>
    public void ShowHealthBarForDuration(float current, float max, bool isHealing = false) // Used for showing the healthbar when taking damage
    {
        if (current <= 0)
        {
            allowCoolDown = true;
        }

        if (healthBar == null || !allowCoolDown) return;

        healthBar.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }

    public void ShowHealthBarForDuration(Transform target, DamageInfo damage) // Used for showing the healthbar when taking damage, called from the event system
    {
        if (healthBar == null || target != transform.parent || !allowCoolDown) return;

        healthBar.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }

    /// <summary>
    /// Hides the enemy health bar if cooldown conditions allow.
    /// </summary>
    public void HideHealthBar()
    {
        if (healthBar == null || isOnCooldown)
        { return; }

        allowCoolDown = true;

        healthBar.gameObject.SetActive(false);
    }

    IEnumerator HideHealthBarAfterDelay() // Coroutine to hide the health bar after a delay, and sets a cooldown to prevent it from hiding immediately after showing
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(displayDuration);
        isOnCooldown = false;

        if (allowCoolDown)
        {
            HideHealthBar();
        }
    }

    private void OnDisable() // Unsubscribes from the events when the object is disabled to prevent memory leaks
    {
        enemyDamage.OnHealthChanged -= ShowHealthBarForDuration;
        Event_System.instance.OnEnemyDamage -= ShowHealthBarForDuration;
    }

    private void OnDestroy() // Unsubscribes from the events when the object is destroyed to prevent memory leaks
    {
        enemyDamage.OnHealthChanged -= ShowHealthBarForDuration;
        Event_System.instance.OnEnemyDamage -= ShowHealthBarForDuration;
    }
}