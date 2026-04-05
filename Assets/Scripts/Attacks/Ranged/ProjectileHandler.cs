using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CapsuleCollider selfHitbox;
    public GameObject target;
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
            // if there is a target shoot towards them
            targetPosistion = target.transform.position;
        }
        else
        {
            // if there is no target shoot forward
            targetPosistion = firePoint.position + firePoint.forward * 1000f;
        }

        Vector3 direction = (targetPosistion - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        GameObject projectileObject = Instantiate(projectile, firePoint.position, rotation);


        projectileObject.GetComponent<Projecile>().direction = direction;
        projectileObject.GetComponent<Projecile>().speed = 30f;

        Physics.IgnoreCollision(projectileObject.GetComponent<Collider>(), selfHitbox);

    }
}
