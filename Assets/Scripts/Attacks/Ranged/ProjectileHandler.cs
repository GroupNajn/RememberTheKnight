using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Handles enemy projectile spawning and aiming.
/// Determines whether to shoot directly at a target
/// or straight ahead if the target is outside the firing angle.
/// </summary>
public class ProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CapsuleCollider selfHitbox;
    [SerializeField] public Transform target;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private EnemyWeaponManager weaponManager;

    void Start()
    {
        selfHitbox = GetComponent<CapsuleCollider>();
        weaponManager = GetComponent<EnemyWeaponManager>();
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ShootingPoint") firePoint = child;
        }
    }

    /// <summary>
    /// Spawns and launches a projectile toward the target.
    /// If the target is outside the allowed firing cone,
    /// the projectile is fired forward instead.
    /// </summary>
    public void ShootProjectile()
    {
        Vector3 targetPosition;

        if (target != null)
        {
            var targetDir = (target.position - firePoint.position).normalized;
            var angle = Mathf.Acos(Vector3.Dot(transform.forward, targetDir));

            if (angle < Mathf.Deg2Rad * 20)
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


        GameObject projectileObject = Instantiate(projectile, firePoint.position, rotation);
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();
        projectileScript.enemyWeaponManager = GetComponent<EnemyWeaponManager>();

        

        projectileObject.GetComponent<Projectile>().direction = direction;
        projectileObject.GetComponent<Projectile>().speed = projectileSpeed;

        Physics.IgnoreCollision(projectileObject.GetComponent<Collider>(), selfHitbox);

    }
}
