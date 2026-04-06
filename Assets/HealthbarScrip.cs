using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScrip : MonoBehaviour
{
    public Slider healthbar;
    public RectTransform lerpingRectTransform;
    [SerializeField] private MonoBehaviour target;// drag Player OR Enemy here
    private Vector2 previousAncorPos;
    private IDamageable damageable;
    [SerializeField]float lerpSpeed = 2f;

    void Start()
    {
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

        if (!lerpCondition) return;
        lerpingRectTransform.anchorMax = Vector2.Lerp(lerpingRectTransform.anchorMax,healthbar.fillRect.anchorMax,Time.deltaTime * lerpSpeed);
        lerpingRectTransform.anchorMin = Vector2.Lerp(lerpingRectTransform.anchorMin,healthbar.fillRect.anchorMin,Time.deltaTime * lerpSpeed);


    }
   
  

    void UpdateHealthBar(float current, float max)
    {

        healthbar.maxValue = max;
        healthbar.value = current;
        if (current <= 0)
        {
            Destroy(healthbar.gameObject);
            damageable.OnHealthChanged -= UpdateHealthBar;
        }
    }  
}
