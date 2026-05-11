using System.Linq;
using UnityEngine;

public class BallistaEnemyAttack : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform target;

    [Header("Parts")]
    private BallistaProjectileHandler projHandler;
    [SerializeField] private Transform crossbowRoot;
    [SerializeField] private Transform crossbow;
    [SerializeField] private Transform bow;
    [SerializeField] private Transform arrow;

    [Header("Settings")]
    
    [SerializeField] private float attackDelayTimer = 1f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDuration = 3f;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float rotationSpeed = 5f;

    private bool isAttacking = false;
    private bool hasShot = false;

    private float attackTimer = 0f;
    private float cooldownTimer = 0f;
    private float delayTimer = -1f;

    [Header("String/Arrow Positions")]
    [SerializeField] private Vector3 bowReady;
    [SerializeField] private Vector3 bowReleased;
    [SerializeField] private Vector3 arrowReady;
    [SerializeField] private Vector3 arrowReleased;

    [Header("Animation/VFX")]
    [SerializeField] private AnimationCurve attackCurve;
    [SerializeField] private ParticleSystem attackParticles;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Transform>().ToList().ForEach(transform => 
        { 
            if (transform.name == "Spine_02") target = transform; 
        });
        projHandler = GetComponent<BallistaProjectileHandler>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (target == null) return;

        if (!IsTargetInRange()) return;

        AimAtPlayer();
        HandleAttackLogic();

    }

    private bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= attackRange;
    }

    private void HandleAttackLogic()
    {
        if (!isAttacking && cooldownTimer <= 0f)
        {
            if (delayTimer <= 0f)
            {
                delayTimer = attackDelayTimer;
                attackParticles.Play();
            }
        }

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
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

        float curveT = attackCurve.Evaluate(t);
        curveT = Mathf.Pow(curveT, 0.8f);

        bow.localPosition = Vector3.Lerp(bowReady, bowReleased, curveT);
        arrow.localPosition = Vector3.Lerp(arrowReady, arrowReleased, curveT);

        if (!hasShot && curveT >= 0.95f)
        {
            projHandler.Shoot();
            hasShot = true;

            cooldownTimer = attackCooldown;
        }

        if (hasShot && curveT <= 0.95f)
        {
            arrow.gameObject.SetActive(true);
        }

        if (t >= 1f)
        {
            isAttacking = false;
            hasShot = false;
        }
    }
}
