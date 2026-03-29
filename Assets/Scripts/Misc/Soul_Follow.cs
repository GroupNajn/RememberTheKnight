using UnityEngine;

public class Soul_Follow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [SerializeField] private float followRange;
    [SerializeField] private float followSpeed;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float growthRate;
    [SerializeField] private float drag;

    private float maxFollowSpeed = 10.0f;
    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;

    private Transform mouth;
    private Droppable droppable;
    private BounceScript bounceScript;

    void Start()
    {
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);
        mouth = GameObject.Find("Jaw").transform;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        droppable = GetComponent<Droppable>();
        bounceScript = GetComponent<BounceScript>();
    }

    void Update()
    {
        if (bounceScript != null && !bounceScript.FinishedBounce)
            return;
        if (player == null)
            return;
        Debug.Log("Soul_Follow took over at frame: " + Time.frameCount);
        distanceVector = player.position - transform.position;

        HoverSinWave();

        if (CanFollow() && droppable.Pickable == PickableState.Pickable)
        {
            if (followSpeed < maxFollowSpeed)
                followSpeed *= Mathf.Exp((growthRate * 0.1f) * Time.deltaTime);

            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.deltaTime);
            followSpeed = 1.0f;
        }

        transform.position += velocity * Time.deltaTime;
    }

    private bool CanFollow()
    {
        float distanceToPlayer = distanceVector.magnitude;
        return player != null && distanceToPlayer <= followRange;
    }

    private void HoverSinWave()
    {
        if (droppable.Pickable != PickableState.Pickable)
            return;

        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * (hoverHeight * 0.001f);
        transform.position += new Vector3(0, y, 0);
    }
}