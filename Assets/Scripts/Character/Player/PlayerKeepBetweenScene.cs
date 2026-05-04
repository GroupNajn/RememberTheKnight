using UnityEngine;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    public static PlayerKeepBetweenScene Instance { get; private set; }

    CharacterController characterController;
    GameObject freeLookCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            characterController = GetComponent<CharacterController>();
            freeLookCamera = GameObject.FindGameObjectWithTag("FreeLookCamera");
        }
    }

    public void Teleport(Transform targetTransform)
    {
        characterController.enabled = false;
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
        characterController.enabled = true;

        CameraTeleporter.Instance.TeleportCameraWithPlayer();
    }
}