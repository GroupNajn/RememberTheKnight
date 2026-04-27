using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            damageable.TakeDamage(9999, contactPoint);
        }
    }
}
