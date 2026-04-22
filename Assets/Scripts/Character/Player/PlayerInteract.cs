using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    PlayerController playerController;
    private IInteractable currentInteractable;
    private IInteractable previousInteractable;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //camera = playerController._playerCamera.GetComponent<Camera>();

    }

    void Update()
    {
        CheckInteractable();
        CheckInteractUI();
    }

    public void OnInteract()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.red, 1f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void CheckInteractUI()
    {
        camera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        

        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            currentInteractable = interactable;

            if (currentInteractable != previousInteractable)
            {
                if (previousInteractable != null)
                    previousInteractable.IsLookedAt = false;

                previousInteractable = currentInteractable;
            }

            currentInteractable.IsLookedAt = true;
            return;
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
                if (!UIManager.Instance.UIMenuActive)
                {

                    UIManager.Instance.OpenInteractiveUI();
                }
                return;
            }
        }

        UIManager.Instance.CloseInteractiveUI();
    }
}