

using UnityEngine;


public class Soul_Follow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Script by Henric 2026-03-14
    [Header("Target")]
    [SerializeField] Transform player;

    [SerializeField] float followRange;

    [SerializeField] float followSpeed;

    [SerializeField] float hoverSpeed;

    [SerializeField] float hoverHeight;

    [SerializeField] private Event_System EventSystem;
    [SerializeField] float drag; // DeAcceleration drag
    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;
    private Transform transform;
    void Start()
    {
        transform = GetComponent<Transform>();
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);

        //if (Player == null) return;
        player = FindAnyObjectByType<PlayerStats>().transform;
        //Initialize(Player);
    }

   

    // Update is called once per frame
    void Update()
    {
        distanceVector = player.transform.position - transform.position;

        HoverSinWave();

        if (CanFollow())
        {
            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;   
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.deltaTime); // A smooth exponential curve in decrease of acceleration
        }

        transform.position += velocity * Time.deltaTime;
    }

    //public void Initialize(GameObject player)
    //{
    //    Player = player;
    //}

    private bool CanFollow()
    {
        float distanceToPlayer = distanceVector.magnitude;
        if (player != null && distanceToPlayer <= followRange)
            return true;
        return false;
    }

    private void HoverSinWave()
    {                                                               // Hoverheight * 0.01f to decrease the scale of the sinus wave
        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * (hoverHeight * 0.001f);
        transform.position += new Vector3(0, y, 0);
    }




}
