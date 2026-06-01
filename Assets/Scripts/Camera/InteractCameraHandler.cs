using FMODUnity;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCameraHandler : MonoBehaviour
{
    /// <summary>
    /// Created by Anton 2026-05-03
    /// Initially created to handle the camera switching when the player interacts with certain objects.
    /// This is used by interactable objects that require a specific camera angle.
    /// This script switches the camera to a predefined position and rotation when the player interacts with the object
    /// And switch back to the free look camera when the interaction is finished.
    /// 
    /// Changed by Anton 2026-05-04
    /// Added the players transform as a default follow target for the interact camera,
    /// so that it will follow the player when no specific target is set.
    /// 
    /// Changed by Anton 2026-05-20
    /// Added functionality to hide the player model when switching to the interact camera, 
    /// this is done by changing the culling mask on main camera.
    /// </summary>

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

    private GameObject mainCamera;
    private Camera mainCameraComponent;

    void Start()
    {
        cameraAnimator = GetComponentInChildren<Animator>();

        interactCam = GameObject.FindGameObjectWithTag("ShopCamera");
        cinemachineInteractCam = interactCam.GetComponent<CinemachineCamera>();
        cinemachinePositionComposer = cinemachineInteractCam.GetComponent<CinemachinePositionComposer>();
        cinemachinePanTilt = cinemachineInteractCam.GetComponent<CinemachinePanTilt>();
        
        freeLookCam = GameObject.FindGameObjectWithTag("FreeLookCamera");
        cinemachineInputAxisController = freeLookCam.GetComponent<CinemachineInputAxisController>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCameraComponent = mainCamera.GetComponent<Camera>();

        if (playerTransform == null) 
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerLookAt");

            cinemachineInteractCam.Follow = playerTransform;
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

        // Set the culling mask to hide the player layer
        int playerLayer = LayerMask.NameToLayer("Player");
        mainCameraComponent.cullingMask &= ~(1 << playerLayer);

        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.cameraWooshEvent, gameObject);
    }

    public void InteractCamReset()
    {
        if (cinemachineInputAxisController == null || cinemachineInteractCam == null || playerTransform == null)
        {
            return;
        }

        // Reset the culling mask to show the player layer again
        int playerLayer = LayerMask.NameToLayer("Player");
        mainCameraComponent.cullingMask |= (1 << playerLayer);

        cinemachineInputAxisController.enabled = true;
        cameraAnimator.Play(stateName: "FreeLookCamera");

        target = null;
        cinemachineInteractCam.Follow = playerTransform;
        cinemachinePanTilt.PanAxis.Value = 0f;
        cinemachinePanTilt.TiltAxis.Value = 0f;
    }
}

