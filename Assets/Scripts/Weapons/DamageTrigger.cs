using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(WeaponStats))]
public class DamageTrigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Anton A 2026-03-16

    WeaponData weaponData;
    float damageAmount;
    HashSet<IDamageable> damagedObjects = new HashSet<IDamageable>();

    private void Start()
    {
        weaponData = GetComponent<WeaponStats>().WeaponData;
        if (weaponData != null)
        {
            damageAmount = weaponData.BaseDamage;
        }
        else
        {
            damageAmount = 999;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello IS DAMAGE");

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damagedObjects.Add(damageable))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            damageable.TakeDamage(damageAmount, contactPoint);
        }
    }

    public void ResetDamage()
    {
        damagedObjects.Clear();
        Debug.Log("Damage reset, ready to damage new targets.");
    }
}