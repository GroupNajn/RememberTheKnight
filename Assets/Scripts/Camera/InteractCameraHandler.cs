using Unity.Cinemachine;
using UnityEngine;


public class InteractCameraHandler : MonoBehaviour
{

    private Animator cameraAnimator;

    private Transform target;

    [Header(header: "Cameras")]
    private GameObject interactCam;
    private CinemachineCamera cinemachineInteractCam;
    private CinemachinePositionComposer cinemachinePositionComposer;

    void Start()
    {
        cameraAnimator = GetComponentInChildren<Animator>();

        interactCam = GameObject.FindGameObjectWithTag("ShopCamera");
        cinemachineInteractCam = interactCam.GetComponent<CinemachineCamera>();
        cinemachinePositionComposer = cinemachineInteractCam.GetComponent<CinemachinePositionComposer>();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InteractCamReset();
        }
    }

    public void InteractCamSwitch(Transform target, InteractCameraPreset preset)
    {
        if (target == null)
        {
            Debug.Log(message: "Target is null. Cannot switch camera.");
            return;
        }

        this.target = target;

        cinemachineInteractCam.Follow = this.target;
        cinemachineInteractCam.transform.rotation = Quaternion.Euler(preset.rotation);
        cinemachinePositionComposer.Composition.ScreenPosition = preset.screenPosition;
        cinemachinePositionComposer.CameraDistance = preset.distance;

        cameraAnimator.Play(stateName: "InteractCamera");
    }

    public void InteractCamReset()
    {
        target = null;
        cinemachinePositionComposer.Composition.ScreenPosition = Vector2.zero;
        cinemachinePositionComposer.transform.rotation = Quaternion.identity;
        cameraAnimator.Play(stateName: "FreeLookCamera");
    }
}
