using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    public InteractableUIData UIData { get => UIData; private set => UIData = value; }
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
