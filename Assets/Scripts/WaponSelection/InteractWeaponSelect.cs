using UnityEngine;

public class InteractWeaponSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    PlayerWeaponManager weaponManager;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        weaponManager = player.GetComponent<PlayerWeaponManager>();
    }

    public void Interact()
    {
        weaponManager.SwitchWeapon();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "[F]: Switch Weapon";
        return UIData;
    }
}