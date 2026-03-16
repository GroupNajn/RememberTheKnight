using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform shooter;
    [SerializeField] Transform target;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30;

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Vector3 targetPosistion;

        if (target != null)
        {
            // if there is a target shoot towards them
            targetPosistion = target.position;
        }
        else
        {
            // if there is no target shoot forward
            targetPosistion = firePoint.position + firePoint.forward * 1000f;
        }

        Vector3 direction = (targetPosistion -  firePoint.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);


        GameObject projectileObject = Instantiate(projectile, firePoint.position, rotation);
        

        projectileObject.GetComponent<Projecile>().direction = direction;
        projectileObject.GetComponent<Projecile>().speed = 30f;

        Physics.IgnoreCollision(projectileObject.GetComponent<Collider>(), shooter.GetComponent<Collider>());
    
    }
}
