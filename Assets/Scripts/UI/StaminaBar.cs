using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    StaminaController staminaController;
    PlayerStats playerStats;
    [SerializeField] Slider staminaBar;
    [SerializeField] float lerpSpeed = 2f;
    public RectTransform lerpingRectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        staminaController = GetComponent<StaminaController>();
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

        playerStats.onStaminaChange += UpdateStaminaBar;

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

    // Update is called once per frame

    void UpdateStaminaBar(float current, float max)
    {
        staminaBar.maxValue = max;
        staminaBar.value = current;
    }
}
