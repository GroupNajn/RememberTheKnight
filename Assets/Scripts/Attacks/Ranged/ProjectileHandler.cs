using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CapsuleCollider selfHitbox;
    [SerializeField] public Transform target;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;

    void Start()
    {
        selfHitbox = GetComponent<CapsuleCollider>();
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ShootingPoint") firePoint = child;
        }
    }

    public void ShootProjectile()
    {
        Vector3 targetPosition;

        if (target != null)
        {
            var firePointForwardXZ = new Vector2(transform.forward.x, transform.forward.z).normalized;
            var targetDir = target.position - firePoint.position;
            var targetXZ = new Vector2(targetDir.x, targetDir.z).normalized;
            var angle = Mathf.Acos(Vector2.Dot(firePointForwardXZ, targetXZ));

            if (angle < Mathf.PI / 4)
            {
                targetPosition = target.transform.position;
            }
            else
            {
                targetPosition = firePoint.position + transform.forward * 1000f;
            }
        }
        else
        {
            // if there is no target shoot forward
            targetPosition = firePoint.position + transform.forward * 1000f;
        }

        Vector3 direction = (targetPosition - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        GameObject projectileObject = Instantiate(projectile, firePoint.position, rotation, gameObject.transform);


        projectileObject.GetComponent<Projectile>().direction = direction;
        projectileObject.GetComponent<Projectile>().speed = projectileSpeed;

        Physics.IgnoreCollision(projectileObject.GetComponent<Collider>(), selfHitbox);

    }
}
