using UnityEngine;

public class InteractFamlySelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    UIManager playerUIManager;

    private bool canShowUI = false;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }

    public void Interact()
    {
        playerUIManager.OpenFamilySelectUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "[F]: Choose Your Faith";
        return UIData;
    }
}