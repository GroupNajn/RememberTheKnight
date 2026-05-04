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

    void Start()
    {
        cameraAnimator = GetComponentInChildren<Animator>();

        interactCam = GameObject.FindGameObjectWithTag("InteractCamera");
        cinemachineInteractCam = interactCam.GetComponent<CinemachineCamera>();
        cinemachinePositionComposer = cinemachineInteractCam.GetComponent<CinemachinePositionComposer>();

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

        Debug.Log($"{cinemachineInteractCam.transform}");
        Debug.Log($"{cinemachineInteractCam.transform.rotation}");
        Debug.Log($"{preset.rotation}");
        cinemachineInteractCam.Follow = this.target;
        cinemachineInteractCam.transform.rotation = Quaternion.Euler(preset.rotation);
        cinemachinePositionComposer.Composition.ScreenPosition = preset.screenPosition;
        cinemachinePositionComposer.CameraDistance = preset.distance;

        cameraAnimator.Play(stateName: "InteractCamera");
    }

    public void InteractCamReset()
    {
        target = playerTransform;
        cinemachineInteractCam.Follow = playerTransform;
        cinemachineInteractCam.transform.position = playerTransform.position;
        cinemachinePositionComposer.Composition.ScreenPosition = Vector2.zero;
        cinemachinePositionComposer.transform.rotation = Quaternion.identity;
        cameraAnimator.Play(stateName: "FreeLookCamera");
    }
}
