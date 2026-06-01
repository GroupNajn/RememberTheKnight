using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates and animates a health bar for either a player or enemy.
/// Supports smooth fill transitions and dynamic resizing.
/// </summary>
public class HealthbarScrip : MonoBehaviour
{
    //edited by Michaëla 2026-05-06
    public Slider healthbar;
    public RectTransform lerpingRectTransform;
    [SerializeField] bool isPlayer = false;
    [SerializeField] private MonoBehaviour target;// drag Player OR Enemy here
    [SerializeField] private RectTransform healthBarTransform;
    [SerializeField] private float widthPerHealth = 3f;
    private Vector2 previousAncorPos;
    private IDamageable damageable;
    [SerializeField] float lerpSpeed = 2f;
    EnemyHealthBarCanvas enemyHealthBarCanvas;

    void Awake() // Gets references to the healthbar, lerpingRectTransform and enemyHealthBarCanvas component
    {
        if(isPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<MonoBehaviour>();
        }
        enemyHealthBarCanvas = GetComponentInParent<EnemyHealthBarCanvas>();
        damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
        {
            Debug.LogError("Target does not implement IDamageable!");
            return;
        }
        damageable.OnHealthChanged += UpdateHealthBar;
        healthbar.maxValue = damageable.MaxHealth;
        healthbar.value = damageable.MaxHealth;
    }


    private void Update() // Lerp the healthbar fill to the target position
    {
        bool lerpCondition = lerpingRectTransform.anchorMax.x > healthbar.fillRect.anchorMax.x || lerpingRectTransform.anchorMin.x < healthbar.fillRect.anchorMin.x;

        if (lerpCondition)
        {
        lerpingRectTransform.anchorMax = Vector2.Lerp(lerpingRectTransform.anchorMax, healthbar.fillRect.anchorMax, Time.deltaTime * lerpSpeed);
        lerpingRectTransform.anchorMin = Vector2.Lerp(lerpingRectTransform.anchorMin, healthbar.fillRect.anchorMin, Time.deltaTime * lerpSpeed);
        }
        else
        {
            lerpingRectTransform.anchorMax = healthbar.fillRect.anchorMax;
            lerpingRectTransform.anchorMin = healthbar.fillRect.anchorMin;
        }
    }


    /// <summary>
    /// Updates health values, resizes the bar when necessary,
    /// and handles enemy health bar visibility.
    /// </summary>
    /// <param name="current">Current health.</param>
    /// <param name="max">Maximum health.</param>
    void UpdateHealthBar(float current, float max) // Updates the healthbar value and max value, and shows the healthbar for a duration if it's an enemy. Also resizes the healthbar based on max health if it's the player. Destroys the healthbar gameobject when health is 0 or below.
    {
        healthbar.maxValue = max;
        healthbar.value = current; 
        if(!isPlayer)
        {
            enemyHealthBarCanvas?.ShowHealthBarForDuration(current,max);
        }
        
        // Resize based on max health
        if (isPlayer && healthBarTransform != null)
        {
            Vector2 size = healthBarTransform.sizeDelta;
            size.x = max * widthPerHealth;
            healthBarTransform.sizeDelta = size;
        }
        if (healthbar.value <= 0) return; // this line was added to prevent the health to disappear. Remove this - 
        if (current <= 0)                 // condition to destroy gameObject when health is 0 or below. 
        {
            Destroy(healthbar.gameObject);
            damageable.OnHealthChanged -= UpdateHealthBar;
        }
        
    }
}
