using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable, IInteractableUI
{
    [SerializeField] private InteractCameraPreset preset;
    private UIManager playerUIManager;

    private bool canShowUI = false;

    private InteractUI_Controller interactUI_Controller;
    private InteractCameraHandler interactCameraHandler;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        interactUI_Controller = GetComponent<InteractUI_Controller>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
    }
    public void Interact()
    {
        playerUIManager.OpenCardSelectUI();
        interactCameraHandler.InteractCamSwitch(transform, preset);
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "Choose your Minor Arcana.";
        return UIData;

    }

    public void ShowUI()
    {
        if (!canShowUI && interactUI_Controller != null) return;

        interactUI_Controller.EnableCanvasObject();

    }

    public void HideUI()
    {

        interactUI_Controller.DisableCanvas();
    }

    public void SetLookedAt(bool value)
    {
        canShowUI = value;


    }
}
