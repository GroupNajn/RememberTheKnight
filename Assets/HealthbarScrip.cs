using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScrip : MonoBehaviour
{
    public Slider healthbar;
    [SerializeField] private MonoBehaviour target;// drag Player OR Enemy here

    private IDamageable damageable;

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

    void UpdateHealthBar(float current, float max)
    {
        healthbar.maxValue = max;
        healthbar.value = current;
    }

  
}
