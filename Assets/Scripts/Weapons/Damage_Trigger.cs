using UnityEngine;

public class Damage_Trigger : MonoBehaviour
{
    // Made by Lukas and Anton A 2026-03-06

    [SerializeField] int damageAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
        }
    }
}