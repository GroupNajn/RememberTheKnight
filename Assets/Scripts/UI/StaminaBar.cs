using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player's stamina UI.
/// Updates stamina values, animates stamina loss,
/// and resizes the bar based on maximum stamina.
/// </summary>
public class StaminaBar : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerManager playerManager;
    [SerializeField] Slider staminaBar;
    [SerializeField] float lerpSpeed = 2f;
    [SerializeField] private RectTransform staminaBarTransform;
    [SerializeField] private float widthPerStamina = 2f;
    public RectTransform lerpingRectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        if (playerStats == null)
        {
            Debug.LogError("StaminaBar: playerStats is missing!");
            return;
        }

        if (staminaBar == null)
        {
            Debug.LogError("StaminaBar: Slider is missing!");
            return;
        }

        playerManager.onStaminaChanged += UpdateStaminaBar;

        // Uppdatera UI direkt
        UpdateStaminaBar(playerStats.currentStamina, playerStats.maxStamina);
    }

    private void Update()
    {
        bool lerpCondition = lerpingRectTransform.anchorMax.x > staminaBar.fillRect.anchorMax.x;
        if (lerpCondition)
        {
            lerpingRectTransform.anchorMax = Vector2.Lerp(lerpingRectTransform.anchorMax, staminaBar.fillRect.anchorMax, Time.deltaTime * lerpSpeed);
        }
        else
        {
            lerpingRectTransform.anchorMax = staminaBar.fillRect.anchorMax;
        }

    }

    /// <summary>
    /// Updates stamina values and adjusts the stamina bar width.
    /// </summary>
    /// <param name="current">Current stamina.</param>
    /// <param name="max">Maximum stamina.</param>
    void UpdateStaminaBar(float current, float max)
    {
        staminaBar.maxValue = max;
        staminaBar.value = current;

        // Resize based on max Stamina
        Vector2 size = staminaBarTransform.sizeDelta;
        size.x = max * widthPerStamina;
        staminaBarTransform.sizeDelta = size;
    }
}
