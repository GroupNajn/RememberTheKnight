using UnityEngine;

public class InteractWeaponSelect : MonoBehaviour , IInteractable, IInteractableUI
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerWeaponManager weaponManager;
    private bool canShowUI = false;

    private InteractUI_Controller interactUI_Controller;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        weaponManager = player.GetComponent<PlayerWeaponManager>();

        interactUI_Controller = GetComponent<InteractUI_Controller>();

    }
    public void Interact()
    {
        weaponManager.SwitchWeapon();
    }

    public void SetLookedAt(bool value)
    {
        canShowUI = value;
    }

    public void ShowUI()
    {
        if (!canShowUI && interactUI_Controller != null) return;

        interactUI_Controller.EnableCanvasObject();
    }
    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "Interact to switch weapon";
        return UIData;
    }

    public void HideUI()
    {
        interactUI_Controller.DisableCanvas();
    }

}
