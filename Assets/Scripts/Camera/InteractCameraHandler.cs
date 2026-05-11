using Unity.Cinemachine;
using UnityEngine;

public class InteractCameraHandler : MonoBehaviour
{
    private Animator cameraAnimator;

    private Transform target;
    private Transform playerTransform;

    [Header(header: "Cameras")]
    private GameObject interactCam;
    private CinemachineCamera cinemachineInteractCam;
    private CinemachinePositionComposer cinemachinePositionComposer;
    private CinemachinePanTilt cinemachinePanTilt;

    private GameObject freeLookCam;
    private CinemachineInputAxisController cinemachineInputAxisController;

    void Start()
    {
        cameraAnimator = GetComponentInChildren<Animator>();

        interactCam = GameObject.FindGameObjectWithTag("ShopCamera");
        cinemachineInteractCam = interactCam.GetComponent<CinemachineCamera>();
        cinemachinePositionComposer = cinemachineInteractCam.GetComponent<CinemachinePositionComposer>();
        cinemachinePanTilt = cinemachineInteractCam.GetComponent<CinemachinePanTilt>();
        
        freeLookCam = GameObject.FindGameObjectWithTag("FreeLookCamera");
        cinemachineInputAxisController = freeLookCam.GetComponent<CinemachineInputAxisController>();

        if (playerTransform == null) 
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerLookAt");

            cinemachineInteractCam.Follow = playerTransform;
        }

        Debug.Log($"interactCam {interactCam.name}, cinemachineInteractCam {cinemachineInteractCam.name}");
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
        cinemachinePanTilt.PanAxis.Value = preset.rotation.y;
        cinemachinePanTilt.TiltAxis.Value = preset.rotation.x;
        cinemachinePositionComposer.Composition.ScreenPosition = preset.screenPosition;
        cinemachinePositionComposer.CameraDistance = preset.distance;

        cameraAnimator.Play(stateName: "InteractCamera");
        cinemachineInputAxisController.enabled = false;
    }

    public void InteractCamReset()
    {
        cinemachineInputAxisController.enabled = true;
        cameraAnimator.Play(stateName: "FreeLookCamera");

        target = null;
        cinemachineInteractCam.Follow = playerTransform;
        cinemachinePanTilt.PanAxis.Value = 0f;
        cinemachinePanTilt.TiltAxis.Value = 0f;
    }
}

