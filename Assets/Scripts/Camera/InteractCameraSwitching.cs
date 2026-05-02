using Unity.Cinemachine;
using UnityEngine;


public class InteractCameraSwitching : MonoBehaviour
{
    public CinemachineCamera playerCam;
    public CinemachineCamera interactCam;

    private Animator cameraAnimator;

    private Transform shopTarget;

    [Header(header: "Cameras")]
    private GameObject shopCam;
    private GameObject cardSelectCam;
    private CinemachineCamera cinemachineShopCam;
    private CinemachineCamera cinemachineCardSelectCam;

    void Start()
    {
        cameraAnimator = GetComponentInChildren<Animator>();

        shopCam = GameObject.FindGameObjectWithTag("ShopCamera");
        //cardSelectCam = GameObject.FindGameObjectWithTag("CardSelectCamera");
        cinemachineShopCam = shopCam.GetComponent<CinemachineCamera>();
        //cinemachineCardSelectCam = cardSelectCam.GetComponent<CinemachineCamera>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            cameraAnimator.Play(stateName: "ShopCamera");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraAnimator.Play(stateName: "FreeLookCamera");
        }

        if (shopTarget == null)
        {
            shopTarget = GameObject.Find("ShopBoard")?.transform;
            cinemachineShopCam.Follow = shopTarget;
        }
    }
}
