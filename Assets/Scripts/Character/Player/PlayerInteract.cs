using Unity.AppUI.UI;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    PlayerController playerController;
    PlayerUIManager playerUIManager;
    LayerMask layerMask = (1 << 9) | ~(1 << 3);

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //camera = playerController._playerCamera.GetComponent<Camera>();
        playerUIManager = FindFirstObjectByType<PlayerUIManager>();

        playerUIManager.CloseInteractiveUI();
    }

    void Update()
    {
        CheckInteractable();
    }

    public void OnInteract()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.red, 1f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractDistance, layerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void CheckInteractable()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractDistance, layerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (!playerUIManager.PlayerUIActive)
                {
                    playerUIManager.OpenInteractiveUI();
                }
                return;
            }
        }

        playerUIManager.CloseInteractiveUI();
    }
}