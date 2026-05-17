using UnityEngine;
using UnityEngine.Splines;

public class DonationMoveSoul : MonoBehaviour
{


    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float moveDuration = 3f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void InstantiateSoul()
    {
        GameObject soul = Instantiate(soulPrefab);

        DonationSoulMover mover = soul.GetComponent<DonationSoulMover>();

        if (mover == null)
            mover = soul.AddComponent<DonationSoulMover>();

        mover.Initialize(spline, moveDuration);
    }
}
