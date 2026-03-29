using UnityEngine;
using UnityEngine.UI;

public class HealthbarScrip : MonoBehaviour
{
    public Slider healthbar;
    [field: SerializeField] public MonoBehaviour target; // drag Player OR Enemy here

    private IDamageable damageable;

    void Start()
    {
        damageable = target as IDamageable; 

        if (damageable == null )
        {
            Debug.LogError("Target does not implement IDamageable!");
            return;
        }
        damageable.OnHealthChanged += UpdateHealthBar;

        UpdateHealthBar(damageable.Health, damageable.MaxHealth);
    }

    void UpdateHealthBar(float current, float max)
    {
        healthbar.maxValue = max;
        healthbar.value = current;
    }
}
