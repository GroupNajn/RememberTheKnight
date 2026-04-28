using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScrip : MonoBehaviour
{
    public Slider healthbar;
    public RectTransform lerpingRectTransform;
    [SerializeField] bool isPlayer = false;
    [SerializeField] private MonoBehaviour target;// drag Player OR Enemy here
    [SerializeField] private RectTransform healthBarTransform;
    [SerializeField] private float widthPerHealth = 3f;
    private Vector2 previousAncorPos;
    private IDamageable damageable;
    [SerializeField] float lerpSpeed = 2f;

    void Start()
    {
        if(isPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<MonoBehaviour>();
        }
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

    private void Update()
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



    void UpdateHealthBar(float current, float max)
    {
        healthbar.maxValue = max;
        healthbar.value = current;

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
