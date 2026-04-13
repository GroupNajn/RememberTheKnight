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
    [SerializeField] private float hoverHeight = 0.08f;
    [SerializeField] private float baseHoverOffset = 0.8f;

    [Header("Landing Transition")]
    [SerializeField] private float moveToBaseDuration = 0.35f;
    [SerializeField] private float blendToHoverDuration = 0.25f;

    [Header("Override Follow")]
    [SerializeField] private float liftSpeed = 1.5f;
    [SerializeField] private float boostSpeed = 50f;

    private bool hasLifted = false;
    private bool liftInitialized = false;
    private float liftTargetY;

    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;

    private Loot loot;
    private BounceScript bounceScript;

    private enum FollowLogic { NotOverriten, Overwritten }
    private FollowLogic followLogic = FollowLogic.NotOverriten;

    private enum HoverState
    {
        WaitingForLanding,
        MovingToBase,
        BlendingToHover,
        Hovering
    }

    private HoverState hoverState = HoverState.WaitingForLanding;

    private Vector3 basePosition;
    private Vector3 lerpStartPosition;
    private Vector3 blendStartPosition;
    private Vector3 initialHoverTarget;

    private float stateTimer = 0f;
    private bool initialized = false;

    void Start()
    {
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);

        loot = GetComponent<Loot>();
        bounceScript = GetComponent<BounceScript>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        Event_System.instance.OnPullAllLoot += OverWriteFollowEnum;
        Event_System.instance.OnResetPullAllLoot += ResetFollowEnum;
    }

    private void OnDestroy()
    {
        Event_System.instance.OnPullAllLoot -= OverWriteFollowEnum;
        Event_System.instance.OnResetPullAllLoot -= ResetFollowEnum;
    }

    void Update()
    {
        if (player == null || loot == null || bounceScript == null)
            return;

        if (!bounceScript.HasLanded)
            return;

        if (!initialized)
        {
            InitializeHoverTransition();
        }

        bool handledMovement = FollowNoDistanceCheck();
        if (handledMovement)
            return;

        switch (hoverState)
        {
            case HoverState.MovingToBase:
                UpdateMoveToBase();
                return;

            case HoverState.BlendingToHover:
                UpdateBlendToHover();
                return;

            case HoverState.Hovering:
                UpdateFollowAndHover();
                return;
        }
    }

    private void InitializeHoverTransition()
    {
        initialized = true;

        basePosition = bounceScript.LastBouncePosition + Vector3.up * baseHoverOffset;

        lerpStartPosition = transform.position;
        stateTimer = 0f;
        hoverState = HoverState.MovingToBase;
    }

    private void UpdateMoveToBase()
    {
        stateTimer += Time.deltaTime;
        float t = Mathf.Clamp01(stateTimer / moveToBaseDuration);

        transform.position = Vector3.Lerp(lerpStartPosition, basePosition, t);

        if (t >= 1f)
        {
            transform.position = basePosition;

            blendStartPosition = transform.position;
            initialHoverTarget = GetHoverPosition();

            stateTimer = 0f;
            hoverState = HoverState.BlendingToHover;
        }
    }

    private void UpdateBlendToHover()
    {
        stateTimer += Time.deltaTime;
        float t = Mathf.Clamp01(stateTimer / blendToHoverDuration);

        transform.position = Vector3.Lerp(blendStartPosition, initialHoverTarget, t);

        if (t >= 1f)
        {
            transform.position = initialHoverTarget;
            hoverState = HoverState.Hovering;
        }
    }

    private void UpdateFollowAndHover()
    {
        distanceVector = player.position - transform.position;
        SetLootFollow();

        Vector3 followMove = velocity * Time.deltaTime;

        basePosition.x += followMove.x;
        basePosition.z += followMove.z;

        transform.position = GetHoverPosition();
    }

    private bool CanFollow()
    {
        return distanceVector.magnitude <= followRange;
    }

    public void SetLootFollow()
    {
        if (loot.Pickable != PickableState.Pickable)
            return;

        if (followLogic == FollowLogic.NotOverriten)
        {
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
    }

    private Vector3 GetHoverPosition()
    {
        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * (hoverHeight * 0.01f);
        return basePosition + new Vector3(0f, y, 0f);
    }

    private bool FollowNoDistanceCheck()
    {
        if (loot.Pickable == PickableState.Pickable && followLogic == FollowLogic.Overwritten)
        {
            if (!liftInitialized)
            {
                liftTargetY = transform.position.y + 2f;
                liftInitialized = true;
                velocity = Vector3.zero;
            }

            if (!hasLifted)
            {
                Vector3 pos = transform.position;
                pos.y = Mathf.Lerp(pos.y, liftTargetY, liftSpeed * Time.deltaTime);
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

        return false;
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
        hoverState = HoverState.WaitingForLanding;
        stateTimer = 0f;
    }
}