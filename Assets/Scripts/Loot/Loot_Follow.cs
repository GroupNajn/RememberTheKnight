using System.Collections;
using UnityEngine;

public class Loot_Follow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [SerializeField] private float followRange;
    [SerializeField] private float followSpeed;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float growthRate;
    [SerializeField] private float drag;
    [SerializeField] private float transitionDuration;
    [SerializeField] private float blendSpeed;
    private bool hasLifted = false;
    private float liftSpeed = 1.5f;
    private float boostSpeed = 50f;
    private float liftTargetY;
    private bool liftInitialized = false;
    private float maxFollowSpeed = 10.0f;
    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;
    private Vector3 currentVelocity;
    private Vector3 launchVelocity;

    private Vector3 hoverVelocity = Vector3.zero;
    private enum FollowLogic { NotOverriten, Overwritten }
    private FollowLogic followLogic = FollowLogic.NotOverriten;

    private Transform mouth;
    private Loot loot;
    private BounceScript bounceScript;

    void Start()
    {
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);
        mouth = GameObject.Find("Jaw").transform;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        loot = GetComponent<Loot>();
        bounceScript = GetComponent<BounceScript>();
        followLogic = FollowLogic.NotOverriten;

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
        if (player == null)
            return;

        if (bounceScript != null && !bounceScript.HasLanded)
            return;

        distanceVector = player.position - transform.position;

        SetLootFollow();

        bool handledMovement = FollowNoDistanceCheck();

        if (!handledMovement)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }

    private bool CanFollow()
    {
        float distanceToPlayer = distanceVector.magnitude;
        return player != null && distanceToPlayer <= followRange;
    }


    public void SetLootFollow()
    {
        if (CanFollow() && loot.Pickable == PickableState.Pickable && followLogic == FollowLogic.NotOverriten)
        {
            if (followSpeed < maxFollowSpeed)
                followSpeed *= Mathf.Exp((growthRate * 0.1f) * Time.deltaTime);
            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;
            HoverSinWave();
        }
        else if (!CanFollow() && loot.Pickable == PickableState.Pickable && followLogic == FollowLogic.NotOverriten)
        {
            HoverSinWave();
            velocity *= Mathf.Exp(-drag * Time.deltaTime); followSpeed = 1.0f;
            
            followSpeed = 1.0f;
        }
    }


    private void HoverSinWave()
    {
        if (loot.Pickable != PickableState.Pickable)
            return;

        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * (hoverHeight * 0.001f);

        // set y-velocity and keep xz velocity
        hoverVelocity = new Vector3(0f, y, 0f);

        velocity += hoverVelocity;
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

            if (!hasLifted) // bool if lift is completed
            {
                Vector3 pos = transform.position;
                pos.y = Mathf.Lerp(pos.y, liftTargetY, liftSpeed * Time.deltaTime);
                transform.position = pos;

                velocity = Vector3.zero;

                if (Mathf.Abs(pos.y - liftTargetY) < 0.05f) // Distance check to see if height distance have been reached
                {
                    pos.y = liftTargetY;
                    transform.position = pos;
                    hasLifted = true;
                }

                return true; // returns true when moved this frame
            }

            dirVector = (player.position - transform.position).normalized;
            velocity = dirVector * boostSpeed;
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
    }


}