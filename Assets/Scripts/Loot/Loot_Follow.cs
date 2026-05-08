using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Loot_Follow_Rigidbody : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Follow")]
    [SerializeField] private float followRange = 5f;
    [SerializeField] private float startFollowSpeed = 1f;
    [SerializeField] private float growthRate = 5f;
    [SerializeField] private float drag = 3f;
    [SerializeField] private float maxFollowSpeed = 10f;

    [Header("Hover")]
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float hoverHeight = 1f;
    [SerializeField] private float baseHoverOffset = 0.8f;

    [Header("Hover Transition")]
    [SerializeField] private float moveToHoverSpeed = 2f;
    [SerializeField] private float arriveThreshold = 0.05f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 10f;
    [SerializeField] private float heightCorrectionSpeed = 3f;

    [Header("Override Follow")]
    [SerializeField] private float liftSpeed = 3f;
    [SerializeField] private float boostSpeed = 50f;

    [Header("Collision Push")]
    [SerializeField] private float collisionPushForce = 2f;

    private Rigidbody rb;
    private Loot loot;
    private BounceScript bounceScript;

    private Transform centerPoint;
    private float centerOffsetY;
    private float hoverOffset;

    private Vector3 basePosition;
    private Vector3 velocity;

    private float currentFollowSpeed;

    private bool initialized;
    private bool isMovingToHover;
    private bool isHovering;

    private bool hasLifted;
    private bool liftInitialized;
    private float liftTargetY;

    private enum FollowLogic
    {
        NotOverwritten,
        Overwritten
    }

    private FollowLogic followLogic = FollowLogic.NotOverwritten;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        loot = GetComponent<Loot>();
        bounceScript = GetComponent<BounceScript>();

        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        currentFollowSpeed = startFollowSpeed;
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);

        centerPoint = FindChildByName("Center");

        if (centerPoint != null)
            centerOffsetY = centerPoint.position.y - transform.position.y;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }

        if (Event_System.instance != null)
        {
            Event_System.instance.OnPullAllSouls += OverWriteFollowEnum;
            Event_System.instance.OnResetPullAllLoot += ResetFollowEnum;
        }
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
        {
            Event_System.instance.OnPullAllSouls -= OverWriteFollowEnum;
            Event_System.instance.OnResetPullAllLoot -= ResetFollowEnum;
        }
    }

    private void FixedUpdate()
    {
        if (player == null || loot == null || bounceScript == null)
            return;

        if (!bounceScript.HasLanded)
        {
            rb.useGravity = true;
            return;
        }

        rb.useGravity = false;
        if (!initialized)
            InitializeHoverTransition();

        if (FollowNoDistanceCheck())
            return;

        if (isMovingToHover)
        {
            MoveToHoverBasePosition();
            return;
        }

        if (isHovering)
        {
            UpdateFollowVelocity();
            UpdateBasePositionXZ();
            CorrectHoverHeight();

            Vector3 targetPosition = GetHoverPosition();
            rb.MovePosition(targetPosition);
        }
    }

    private void InitializeHoverTransition()
    {
        initialized = true;

        Vector3 wantedCenterPosition = bounceScript.LastBouncePosition + Vector3.up * baseHoverOffset;

        basePosition = wantedCenterPosition - Vector3.up * centerOffsetY;

        isMovingToHover = true;
        isHovering = false;

        rb.linearVelocity = Vector3.zero;
    }

    private void MoveToHoverBasePosition()
    {
        Vector3 newPosition = Vector3.MoveTowards(rb.position,basePosition,moveToHoverSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPosition);

        if (centerPoint == null)
            return;

        Vector3 targetCenterPosition = bounceScript.LastBouncePosition + Vector3.up * baseHoverOffset;

        if (Vector3.Distance(centerPoint.position, targetCenterPosition) <= arriveThreshold)
        {
            isMovingToHover = false;
            isHovering = true;
        }
    }

    private void CorrectHoverHeight()
    {
        if (centerPoint == null)
            return;

        Vector3 rayStart = centerPoint.position + Vector3.up;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundCheckDistance, bounceScript.EnvironmentMask))
        {
            Debug.DrawRay(rayStart, Vector3.down * groundCheckDistance, Color.red);

            float wantedCenterY = hit.point.y + baseHoverOffset;
            float currentCenterY = centerPoint.position.y;

            float yDifference = wantedCenterY - currentCenterY;

            basePosition.y += yDifference * heightCorrectionSpeed * Time.fixedDeltaTime;
        }
    }

    private Vector3 GetHoverPosition()
    {
        float hoverY = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight * 0.01f;

        return basePosition + Vector3.up * hoverY;
    }

    private void UpdateFollowVelocity()
    {
        Vector3 playerFlat = new Vector3(player.position.x, rb.position.y, player.position.z);
        Vector3 distanceVector = playerFlat - rb.position;

        if (loot.Pickable != PickableState.Pickable)
            return;

        if (followLogic != FollowLogic.NotOverwritten)
            return;

        if (distanceVector.magnitude <= followRange)
        {
            currentFollowSpeed *= Mathf.Exp((growthRate * 0.1f) * Time.fixedDeltaTime);
            currentFollowSpeed = Mathf.Min(currentFollowSpeed, maxFollowSpeed);

            velocity = distanceVector.normalized * currentFollowSpeed;
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.fixedDeltaTime);
            currentFollowSpeed = startFollowSpeed;
        }
    }

    private void UpdateBasePositionXZ()
    {
        Vector3 move = velocity * Time.fixedDeltaTime;

        basePosition.x += move.x;
        basePosition.z += move.z;
    }

    private bool FollowNoDistanceCheck()
    {
        if (loot.Pickable != PickableState.Pickable)
            return false;

        if (followLogic != FollowLogic.Overwritten)
            return false;

        if (!liftInitialized)
        {
            liftTargetY = rb.position.y + 2f;
            liftInitialized = true;
            velocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }

        if (!hasLifted)
        {
            Vector3 target = rb.position;
            target.y = Mathf.MoveTowards(rb.position.y, liftTargetY, liftSpeed * Time.fixedDeltaTime);

            rb.MovePosition(target);

            if (Mathf.Abs(target.y - liftTargetY) < 0.05f)
                hasLifted = true;

            return true;
        }

        Vector3 direction = (player.position - rb.position).normalized;
        Vector3 newPosition = rb.position + direction * boostSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHovering)
            return;

        if (collision.contactCount <= 0)
            return;

        Vector3 normal = collision.GetContact(0).normal;

        rb.AddForce(normal * collisionPushForce, ForceMode.Impulse);
    }

    private Transform FindChildByName(string childName)
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.name == childName)
                return t;
        }

        return null;
    }

    private void OverWriteFollowEnum()
    {
        followLogic = FollowLogic.Overwritten;
    }

    private void ResetFollowEnum()
    {
        followLogic = FollowLogic.NotOverwritten;

        hasLifted = false;
        liftInitialized = false;

        initialized = false;
        isMovingToHover = false;
        isHovering = false;

        velocity = Vector3.zero;
        currentFollowSpeed = startFollowSpeed;

        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }
}