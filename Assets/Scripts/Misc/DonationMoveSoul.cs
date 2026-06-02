using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class DonationMoveSoul : MonoBehaviour
{


    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float moveDuration = 3f;
    [SerializeField] private Transform playerTransform;
    private bool hasInserted = false;
    void Start()
    {
        playerTransform = GameObject.Find("PlayerLookAt").transform;
    }

    void Update()
    {

    }


    public void InstantiateSoul()
    {
        Vector3 localPosition = spline.transform.InverseTransformPoint(playerTransform.position);

        BezierKnot playerKnot = new BezierKnot((float3)localPosition);
        if (!hasInserted)
        {
            spline.Spline.Insert(0, playerKnot);
            hasInserted = true;
        }
        else spline.Spline[0] = playerKnot;

            GameObject soul = Instantiate(soulPrefab);
        DonationSoulMover mover = soul.GetComponent<DonationSoulMover>();

        if (mover == null)
            mover = soul.AddComponent<DonationSoulMover>();


        mover.Initialize(spline, moveDuration);
        
    }
}
