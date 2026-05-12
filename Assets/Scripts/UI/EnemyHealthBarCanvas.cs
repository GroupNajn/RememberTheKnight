using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarCanvas : MonoBehaviour
{
    //Editied by Michaëla 2026-05-06
    [SerializeField] float displayDuration = 2f; // Duration to show the health bar after taking damage

    EnemyDamage enemyDamage;
    Slider healthBar;
    bool isOnCooldown = false;
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

    public void ShowHealthBar() // Used for locking on target
    {
        healthBar.gameObject.SetActive(true);
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }
    public void ShowHealthBarForDuration(float current, float max) // Used for showing the healthbar when taking damage
    {
        if (healthBar == null) return;

        healthBar.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }
    public void ShowHealthBarForDuration(Transform target, DamageInfo damage) // Used for showing the healthbar when taking damage, called from the event system
    {
        if (healthBar == null) return;

        healthBar.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideHealthBarAfterDelay());
    }
    public void HideHealthBar()
    {
        if (healthBar == null || isOnCooldown)
            return;

        healthBar.gameObject.SetActive(false);
    }

    IEnumerator HideHealthBarAfterDelay() // Coroutine to hide the health bar after a delay, and sets a cooldown to prevent it from hiding immediately after showing
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(displayDuration);
        isOnCooldown = false;
        HideHealthBar();
    }

    private void OnDisable() // Unsubscribes from the events when the object is disabled to prevent memory leaks
    {
        Event_System.instance.OnEnemyDamage -= ShowHealthBarForDuration;
    }

    private void OnDestroy() // Unsubscribes from the events when the object is destroyed to prevent memory leaks
    {
        Event_System.instance.OnEnemyDamage -= ShowHealthBarForDuration;
    }
}