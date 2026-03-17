

using System.Runtime.InteropServices;
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

    [SerializeField] float growthRate;
    [SerializeField] float drag; // DeAcceleration drag
    private float maxFollowSpeed = 10.0f;
    private float hoverOffset;
    private Vector3 dirVector;
    private Vector3 distanceVector;
    private Vector3 velocity;
    private Transform transform;
    private Transform mouth;
    void Start()
    {
        transform = GetComponent<Transform>();
        hoverOffset = Random.Range(0f, Mathf.PI * 2f);
        mouth = GameObject.Find("Jaw").transform;
        //if (Player == null) return;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

   

    // Update is called once per frame
    void Update()
    {
        distanceVector = player.transform.position - transform.position;

        HoverSinWave();

        if (CanFollow())
        {
            
            if(followSpeed < maxFollowSpeed)
            followSpeed *= Mathf.Exp((growthRate * 0.1f) * Time.deltaTime);
            dirVector = distanceVector.normalized;
            velocity = dirVector * followSpeed;   
        }
        else
        {
            velocity *= Mathf.Exp(-drag * Time.deltaTime); // A smooth exponential curve in decrease of acceleration
            followSpeed = 1.0f;
        }

        transform.position += velocity * Time.deltaTime;
    }
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
