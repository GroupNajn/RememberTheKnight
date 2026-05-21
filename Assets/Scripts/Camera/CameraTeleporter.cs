using Unity.Cinemachine;
using UnityEngine;

public class CameraTeleporter : MonoBehaviour
{
    public static CameraTeleporter Instance { get; private set; }

    private CinemachineCamera cinemachineFreeLookCam;
    private CinemachineCamera cinemachineHardLockCam;
    private Transform playerTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameObject freeLookCam = GameObject.FindGameObjectWithTag("FreeLookCamera");
        GameObject hardlockCam = GameObject.FindGameObjectWithTag("HardLockCamera");

        cinemachineFreeLookCam = freeLookCam.GetComponent<CinemachineCamera>();
        cinemachineHardLockCam = hardlockCam.GetComponent<CinemachineCamera>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerLookAt");
    }

    public void TeleportCameraWithPlayer()
    {
        if (playerTransform == null)
            return;

        // Force update the Cinemachine cameras to match the player's position
        cinemachineFreeLookCam.PreviousStateIsValid = false;
        cinemachineHardLockCam.PreviousStateIsValid = false;

        // Manually set the camera position to match the follow target
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = playerTransform.position + new Vector3(0, 1, 0);
        }
    }
}
