using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    CardSelectionUI cardSelectionUI;

    void Awake()
    {
        cardSelectionUI = FindFirstObjectByType<CardSelectionUI>();

    }
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        cardSelectionUI.OpenCardSelectUI();
    }
}
