using Unity.AppUI.UI;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    [SerializeField] GameObject interactUI;
    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //camera = playerController._playerCamera.GetComponent<Camera>();
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
        if (Physics.Raycast(ray, out hit, InteractDistance,3))
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

        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactUI.SetActive(true);
                return;
            }
        }

        interactUI.SetActive(false);
    }
}