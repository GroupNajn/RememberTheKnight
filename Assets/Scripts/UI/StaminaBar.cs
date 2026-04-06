using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    StaminaController staminaController;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Slider staminaBar;
    [SerializeField] float lerpSpeed = 2f;
    public RectTransform lerpingRectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        lerpingRectTransform.anchorMax = Vector2.Lerp(lerpingRectTransform.anchorMax, staminaBar.fillRect.anchorMax, Time.deltaTime * lerpSpeed);
    }

    // Update is called once per frame

    void UpdateStaminaBar(float current, float max)
    {
        staminaBar.maxValue = max;
        staminaBar.value = current;
    }
}
