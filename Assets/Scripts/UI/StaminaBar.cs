using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("SHOULD HAVE UPDATED");
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

       // Debug.Log("STAMINA BAR UPDATED");
        staminaBar.maxValue = max;
        staminaBar.value = current;

        // Resize based on max Stamina
        Vector2 size = staminaBarTransform.sizeDelta;
        size.x = max * widthPerStamina;
        staminaBarTransform.sizeDelta = size;
    }
}
