using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    public bool IsInteractable { get; set; } = false;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        playerUIManager.OpenCardSelectUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        return UIData;
    }

}
