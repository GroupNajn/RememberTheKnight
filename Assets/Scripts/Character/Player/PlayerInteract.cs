using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;
    public float InteractDistance = 8f;
    PlayerController playerController;
    private IInteractableUI currentUI;
    private IInteractableUI previousUI;


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
        if(Time.timeScale <= 0 && currentUI != null && previousUI != null)
        {
            currentUI.SetLookedAt(false);
            currentUI.HideUI();
            previousUI.SetLookedAt(false);
            previousUI.HideUI();
            return;
        }
        

        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            
            IInteractableUI interactableUI = hit.collider.GetComponentInParent<IInteractableUI>();

            if (interactableUI == null) return;

            currentUI = interactableUI;

            if (currentUI != previousUI)
            {
                if (previousUI != null)
                {
                    previousUI.SetLookedAt(false);
                    previousUI.HideUI();
                    
                }

                previousUI = currentUI;
            }
            if (Time.timeScale <= 0) return;
            currentUI.SetLookedAt(true);
            currentUI.ShowUI();
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