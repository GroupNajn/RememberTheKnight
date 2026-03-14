

using UnityEngine;


public class Soul_Follow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Target")]
    [SerializeField] GameObject Player;
    [Header("Follow_Distance")]
    [SerializeField] float followRange;
    [Header("Follow_Speed")]
    [SerializeField] float followSpeed;
    [Header("Hover_Speed")]
    [SerializeField] float hoverSpeed;
    [Header("Hover_Height")]
    [SerializeField] float hoverHeight;
    [Header("Drag")]
    [SerializeField] float drag;
    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;
    private Transform transform;

    void Start()
    {
        transform = GetComponent<Transform>();
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    // Update is called once per frame
    void Update()
    {
        distanceVector = Player.transform.position - transform.position;

        HoverSinWave();

        if (CanFollow())
        {
            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;   
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.deltaTime);
        }

        transform.position += velocity * Time.deltaTime;
    }

    private bool CanFollow()
    {
        float distanceToPlayer = distanceVector.magnitude;
        if (Player != null && distanceToPlayer <= followRange)
            return true;
        return false;
    }

    private void HoverSinWave()
    {
        float y = Mathf.Sin(Time.time * hoverSpeed + hoverOffset) * hoverHeight;
        transform.position += new Vector3(0, y, 0);
       
    }




}
