using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable, IInteractableUI
{
    UIManager playerUIManager;

    private bool canShowUI = false;

    private InteractUI_Controller interactUI_Controller;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        interactUI_Controller = GetComponent<InteractUI_Controller>();

    }
    public void Interact()
    {
        playerUIManager.OpenCardSelectUI();
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
