using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    public string InfoString { get; private set; } = null;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        playerUIManager.OpenCardSelectUI();
    }
}
