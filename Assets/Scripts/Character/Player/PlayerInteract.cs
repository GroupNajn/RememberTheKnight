using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    public float InteractDistance = 8f;
    PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        CheckInteractable();
    }

    public void OnInteract()
    {
        playerCamera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
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

    public void CheckInteractable()
    {
        playerCamera = playerController._playerCamera.GetComponent<Camera>();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractDistance, 3))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (!UIManager.Instance.UIMenuActive)
                {
                    string interactableUIText = "[F]";
                    if (interactable is IInteractableUIText)
                    {
                        IInteractableUIText interactableUI = (IInteractableUIText)interactable;
                        interactableUIText = interactableUI.GetUIData().InfoText;
                    }
                    UIManager.Instance.OpenInteractiveUI(interactableUIText);
                }
                return;
            }
        }

        UIManager.Instance.CloseInteractiveUI();
    }
}