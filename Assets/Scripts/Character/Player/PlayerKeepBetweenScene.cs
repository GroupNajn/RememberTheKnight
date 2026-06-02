using UnityEngine;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-03-24
    /// Player doesn't get destroyed when swithching scene and is moved to the spawn point in the new scene
    /// 
    /// Updated by Lukas 2026-04-02
    /// Removed Start and moved awake to the top 
    /// 
    /// Updated by Lukas 2026-04-09
    /// Added SetPlayerPosition coroutine
    /// 
    /// Updated by Lukas 2026-04-10
    /// Switched to moving player when a scene is loaded instead of calling a couroutine
    /// 
    /// Updated by Lukas 2026-04-13
    /// Made teleport method
    /// 
    /// Updated by Lukas 2026-04-15
    /// Teleport method turns off character controller to move correctly
    /// 
    /// Updated by Lukas 2026-05-04
    /// Removed unused methods and made the camera follow player when teleporting
    /// </summary>

    public static PlayerKeepBetweenScene Instance { get; private set; }

    CharacterController characterController;

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