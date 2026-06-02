using FMODUnity;
using System.Linq;
using UnityEngine;

public class BallistaProjectileHandler : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-4-17
    /// Initially created to handle the projectile shooting of the ballista enemy.
    /// </summary>
    [Header("Setup")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Transform arrowVisual;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 25f;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Transform>().ToList().ForEach(transform =>
        {
            if (transform.name == "Spine_02") target = transform;
        });
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "ShootingPoint") firePoint = child;
        }
    }

    public void Shoot()
    {
        if (target == null) return;

        arrowVisual.gameObject.SetActive(false);

        Vector3 direction = (target.position - firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, rotation);
        Projectile projectileScript = proj.GetComponent<Projectile>();
        projectileScript.enemyWeaponManager = GetComponent<EnemyWeaponManager>();
        proj.gameObject.SetActive(true);

        proj.GetComponent<Projectile>().direction = direction;
        proj.GetComponent<Projectile>().speed = projectileSpeed;

        
           RuntimeManager.PlayOneShotAttached (gameObject.GetComponent<BallistaEnemy>().ballistaShootEvent, firePoint.gameObject);
           RuntimeManager.PlayOneShotAttached (gameObject.GetComponent<BallistaEnemy>().ballistaLoadEvent, firePoint.gameObject);


        
    }
}
