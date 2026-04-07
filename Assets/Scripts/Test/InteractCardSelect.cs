using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    PlayerUIManager playerUIManager;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<PlayerUIManager>();
    }
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        playerUIManager.OpenCardSelectUI();
    }
}
