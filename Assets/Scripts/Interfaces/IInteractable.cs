using UnityEngine;

public interface IInteractable
{
    void Interact();

    public bool IsLookedAt { get; set; }

    InteractableUIData GetUIData();

}
