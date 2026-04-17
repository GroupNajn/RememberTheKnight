using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform target;

    [Header("Parts")]
    private ProjectileHandler projHandler;
    [SerializeField] private Transform crossbowRoot;
    [SerializeField] private Transform crossbow;
    [SerializeField] private Transform bow;
    [SerializeField] private Transform arrow;

    [Header("Settings")]
    private bool isAttacking = false;
    private float attackTimer = 0f;

    [SerializeField] private float attackDuration = 3f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("String/Arrow Positions")]
    [SerializeField] private Vector3 bowReady;
    [SerializeField] private Vector3 bowReleased;
    [SerializeField] private Vector3 arrowReady;
    [SerializeField] private Vector3 arrowReleased;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        projHandler = GetComponent<ProjectileHandler>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            AimAtPlayer();

            if (!isAttacking)
            {
                StartAttack();
            }
        }

        if (isAttacking)
        {
            AttackPlayer();
        }
    }
    private void AimAtPlayer()
    {
        Vector3 direction = target.position - crossbowRoot.position;

        Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);
        Quaternion rootRotation = Quaternion.LookRotation(horizontalDirection);
        crossbowRoot.rotation = Quaternion.Slerp(crossbowRoot.rotation, rootRotation, rotationSpeed * Time.deltaTime);

        Vector3 localDirection = crossbowRoot.InverseTransformDirection(direction);

        float verticalAngle = Mathf.Atan2(localDirection.y, localDirection.z) * Mathf.Rad2Deg;

        Quaternion verticalRotation = Quaternion.Euler(-verticalAngle, 0f, 0f);

        crossbow.localRotation = Quaternion.Slerp(crossbow.localRotation, verticalRotation, rotationSpeed * Time.deltaTime);
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = 0f;
    }

    private void AttackPlayer()
    {
        attackTimer += Time.deltaTime;
        float t = attackTimer / attackDuration;

        bow.localPosition = Vector3.Lerp(bowReady, bowReleased, t);

        arrow.localPosition = Vector3.Lerp(arrowReady, arrowReleased, t);

        if (t >= 1f)
        {
            projHandler.ShootProjectile();

            bow.localPosition = bowReady;
            arrow.localPosition = arrowReady;

            isAttacking = false;
        }
    }
}
