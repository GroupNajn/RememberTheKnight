using UnityEngine;

public interface IInteractable
{
    void Interact();

    public bool IsInteractable { get; set; }

    InteractableUIData GetUIData();

}
