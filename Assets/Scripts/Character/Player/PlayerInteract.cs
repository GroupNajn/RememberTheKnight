using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera playerCamera;
    public float InteractDistance = 8f;
    PlayerController playerController;
    [SerializeField] private LayerMask interactMask;

    private HighlightTarget highLight;

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
        if (Physics.Raycast(ray, out hit, InteractDistance, interactMask))
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

        if (Physics.Raycast(ray, out hit, InteractDistance, interactMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                HighlightTarget newHighLight = hit.collider.GetComponent<HighlightTarget>();

                if (newHighLight != highLight)
                {
                    if (highLight != null)
                        highLight.SetHighlight(false);

                    highLight = newHighLight;

                    if (highLight != null)
                        highLight.SetHighlight(true);
                }

                if (!UIManager.Instance.UIMenuActive)
                {
                    string keybind = InputManager.Instance.SetInteractBinding();
                    string interactableUIText = $"[{keybind}]  ";
                    if (interactable is IInteractableUIText)
                    {
                        IInteractableUIText interactableUI = (IInteractableUIText)interactable;

                        if (!interactableUI.GetUIData().CanInteract)
                        {
                            interactableUIText = "";
                        }

                        interactableUIText = interactableUIText + interactableUI.GetUIData().InfoText;
                    }
                    UIManager.Instance.OpenInteractiveUI(interactableUIText);
                }
                return;
            }
        }

        ClearHighLight();

        UIManager.Instance.CloseInteractiveUI();
    }

    public void ClearHighLight()
    {
        if (highLight != null)
        {
            highLight.SetHighlight(false);
            highLight = null;
        }
    }
}