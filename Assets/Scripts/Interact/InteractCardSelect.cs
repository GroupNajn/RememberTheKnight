using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }
    public void Interact()
    {
        playerUIManager.OpenCardSelectUI();
    }
}
