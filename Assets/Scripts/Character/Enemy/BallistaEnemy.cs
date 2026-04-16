using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Parts")]
    [SerializeField] private Transform crossbowRoot;
    [SerializeField] private Transform crossbow;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 5f;  
    [SerializeField] private float attackRange = 10f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            AimAtPlayer();
        }
    }
    private void AimAtPlayer()
    {
        Vector3 direction = target.position - crossbowRoot.position;

        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
        Quaternion yawRotation = Quaternion.LookRotation(flatDirection);
        crossbowRoot.rotation = Quaternion.Slerp(crossbowRoot.rotation, yawRotation, rotationSpeed * Time.deltaTime);

        Vector3 localDir = crossbowRoot.InverseTransformDirection(direction);

        float pitchAngle = Mathf.Atan2(localDir.y, localDir.z) * Mathf.Rad2Deg;

        Quaternion pitchRotation = Quaternion.Euler(-pitchAngle, 0f, 0f);

        crossbow.localRotation = Quaternion.Slerp(crossbow.localRotation, pitchRotation, rotationSpeed * Time.deltaTime);
    }
}
