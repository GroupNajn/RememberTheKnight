using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    public bool IsLookedAt { get; set; } = false;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
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

}
