using UnityEngine;

public class Loot_Follow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Follow")]
    [SerializeField] private float followRange = 5f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private float growthRate = 5f;
    [SerializeField] private float drag = 3f;
    [SerializeField] private float maxFollowSpeed = 10f;

    [Header("Hover")]
    [SerializeField] private float hoverSpeed = 1f;
    [SerializeField] private float hoverHeight = 1f;
    [SerializeField] private float baseHoverOffset = 0.8f;

    [Header("Hover Transition")]
    [SerializeField] private float moveToHoverSpeed = 0.5f;
    [SerializeField] private float arriveThreshold = 0.005f;

    [Header("Override Follow")]
    [SerializeField] private float liftSpeed = 1.5f;
    [SerializeField] private float boostSpeed = 50f;

    private Coroutine heightCheckRoutine;

    [SerializeField] private LayerMask environmentMask;
    [SerializeField] private float heightCheckInterval = 1f;
    [SerializeField] private float groundCheckDistance = 10f;
    [SerializeField] private float heightCorrectionSpeed = 5f;


    private bool initialized = false;
    private bool isMovingToHover = false;
    private bool isHovering = false;
    private bool isAdjustingHeight = false;

    private bool hasLifted = false;
    private bool liftInitialized = false;

    private float liftTargetY;
    private float hoverOffset;

    private Transform centerPoint;
    private float centerOffsetY;

    private Vector3 basePosition;
    private Vector3 velocity;
    private Vector3 distanceVector;
    private Vector3 dirVector;

    private Loot loot;
    private BounceScript bounceScript;

    private enum FollowLogic
    {
        NotOverriten,
        Overwritten
    }

    private FollowLogic followLogic = FollowLogic.NotOverriten;

    private void Start()
    {
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);

        loot = GetComponent<Loot>();
        bounceScript = GetComponent<BounceScript>();

        centerPoint = FindChildByName("Center");

        if (centerPoint != null)
        {
            centerOffsetY = centerPoint.position.y - transform.position.y;
        }

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

    private void Update()
    {
        if (player == null || loot == null || bounceScript == null)
            return;

        if (!bounceScript.HasLanded)
            return;

        if (!initialized)
        {
            InitializeHoverTransition();
        }

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
            MoveToNewHoverPosition();

            transform.position = GetHoverPosition();
        }
    }

    private void InitializeHoverTransition()
    {
        initialized = true;

        Vector3 wantedCenterPosition = bounceScript.LastBouncePosition + Vector3.up * baseHoverOffset;

        basePosition = wantedCenterPosition - Vector3.up * centerOffsetY;

        isMovingToHover = true;
        isHovering = false;
    }

    private void MoveToHoverBasePosition()
    {
        transform.position = Vector3.MoveTowards(transform.position,basePosition,moveToHoverSpeed * Time.deltaTime);

        if (centerPoint == null)
            return;

        Vector3 currentCenterPosition = centerPoint.position;
        Vector3 targetCenterPosition = bounceScript.LastBouncePosition + Vector3.up * baseHoverOffset;

        if (Vector3.Distance(currentCenterPosition, targetCenterPosition) <= arriveThreshold)
        {
            isMovingToHover = false;
            isHovering = true;
        }
    }

    private void MoveToNewHoverPosition()
    {
        if (centerPoint == null)
            return;

        Vector3 rayStart = centerPoint.position + Vector3.up * 1f;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f, bounceScript.EnvironmentMask))
        {
            Debug.DrawRay(rayStart, Vector3.down * 10f, Color.red);

            float wantedCenterY = hit.point.y + baseHoverOffset;
            float currentCenterY = centerPoint.position.y;

            float yDifference = wantedCenterY - currentCenterY;

            // Flytta basePosition mot rätt höjd i båda riktningar
            basePosition.y += yDifference * moveToHoverSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHovering) return;
        ContactPoint contact = collision.contacts[0];

        Vector3 normal = contact.normal;

        float pushForce = 0.40f;

        transform.position += normal * pushForce;
    }

    private Vector3 GetHoverPosition()
    {
        float y =Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight * 0.01f;

        return basePosition + new Vector3(0f, y, 0f);
    }

    private void UpdateFollowVelocity()
    {
        Vector3 playerFlat = new Vector3(player.position.x, transform.position.y, player.position.z);
        distanceVector = playerFlat - transform.position;
        SetLootFollow();
    }

    private void UpdateBasePositionXZ()
    {
        Vector3 followMove = velocity * Time.deltaTime;

        basePosition.x += followMove.x;
        basePosition.z += followMove.z;
    }

    private bool CanFollow()
    {
        return distanceVector.magnitude <= followRange;
    }

    private void SetLootFollow()
    {
        if (loot.Pickable != PickableState.Pickable)
            return;

        if (followLogic != FollowLogic.NotOverriten)
            return;

        if (CanFollow())
        {
            if (followSpeed < maxFollowSpeed)
            {
                followSpeed *= Mathf.Exp((growthRate * 0.1f) * Time.deltaTime);
                followSpeed = Mathf.Min(followSpeed, maxFollowSpeed);
            }

            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.deltaTime);
            followSpeed = 1f;
        }
    }

    private bool FollowNoDistanceCheck()
    {
        if (loot.Pickable != PickableState.Pickable)
            return false;

        if (followLogic != FollowLogic.Overwritten)
            return false;

        if (!liftInitialized)
        {
            liftTargetY = transform.position.y + 2f;
            liftInitialized = true;
            velocity = Vector3.zero;
        }

        if (!hasLifted)
        {
            Vector3 pos = transform.position;

            pos.y = Mathf.Lerp( pos.y, liftTargetY ,liftSpeed * Time.deltaTime);

            transform.position = pos;
            velocity = Vector3.zero;

            if (Mathf.Abs(pos.y - liftTargetY) < 0.05f)
            {
                pos.y = liftTargetY;
                transform.position = pos;
                hasLifted = true;
            }

            return true;
        }

        dirVector = (player.position - transform.position).normalized;
        velocity = dirVector * boostSpeed;

        transform.position += velocity * Time.deltaTime;
        return true;
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
        followLogic = FollowLogic.NotOverriten;

        hasLifted = false;
        liftInitialized = false;

        initialized = false;
        isMovingToHover = false;
        isHovering = false;

        velocity = Vector3.zero;
        followSpeed = 1f;
    }
}