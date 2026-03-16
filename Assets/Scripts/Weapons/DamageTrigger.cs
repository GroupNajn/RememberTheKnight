using UnityEngine;

[RequireComponent(typeof(WeaponStats))]
public class DamageTrigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Anton A 2026-03-16

    WeaponData weaponData;
    float damageAmount;

    private void Start()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        if (weaponData != null)
        {
            damageAmount = weaponData.BaseDamage;
        }
        else
        {
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }
    }
}