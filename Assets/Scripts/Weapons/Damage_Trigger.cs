using UnityEngine;

public class Damage_Trigger : MonoBehaviour
{
    // Made by Lukas and Anton B 2026-03-06

    [SerializeField] int damageAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }
    }
}