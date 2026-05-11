using System.Collections;
using UnityEngine;
public interface IDamageable
{
    // Made by Lukas and Anton B 2026-03-06
    // Updated by Lukas and Jonatan 2026-03-16

    public float MaxHealth { get;}
    public float Health { get; }

    System.Action<float, float> OnHealthChanged { get; set; }

    public bool CanTakeDamage { get; }

    //IEnumerator DamageCoolDown(int damageDelay); // Uncomment if you want to use a cooldown for taking damage, but currently not used in the project
    public void TakeDamage(DamageInfo damageInfo, Vector3 contactPoint);
    public void Death();
}