using UnityEngine;

public class DeathZone : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-04-24
    /// Initially created as a component to kill the player when they fall into a death zone
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            DamageInfo damageInfo = new DamageInfo(9999f);
            damageable.TakeDamage(damageInfo, contactPoint);
        }
    }
}
