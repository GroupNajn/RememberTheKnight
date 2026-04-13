using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CapsuleCollider selfHitbox;
    public Transform target;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30;

    void Start()
    {
        selfHitbox = GetComponent<CapsuleCollider>();

    }

    public void ShootProjectile()
    {
        Vector3 targetPosistion;

        if (target != null)
        {
            var firePointForwardXZ = new Vector2(transform.forward.x, transform.forward.z).normalized;
            var targetDir = target.position - firePoint.position;
            var targetXZ = new Vector2(targetDir.x, targetDir.z).normalized;
            var angle = Mathf.Acos(Vector2.Dot(firePointForwardXZ, targetXZ));

            if (angle < Mathf.PI / 4)
            {
                targetPosistion = target.transform.position;
            }
            else
            {
                targetPosistion = firePoint.position + transform.forward * 1000f;
            }
        }
        else
        {
            // if there is no target shoot forward
            targetPosistion = firePoint.position + transform.forward * 1000f;
        }

        Vector3 direction = (targetPosistion - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        GameObject projectileObject = Instantiate(projectile, firePoint.position, rotation);


        projectileObject.GetComponent<Projecile>().direction = direction;
        projectileObject.GetComponent<Projecile>().speed = 30f;

        Physics.IgnoreCollision(projectileObject.GetComponent<Collider>(), selfHitbox);

    }
}
