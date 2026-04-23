using UnityEngine;

public interface IInteractableUI
{

    void ShowUI();
    void HideUI();

    void SetLookedAt(bool value);

    InteractableUIData GetUIData();



}
