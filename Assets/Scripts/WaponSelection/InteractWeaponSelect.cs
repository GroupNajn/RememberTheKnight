using UnityEngine;

public class InteractWeaponSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    PlayerWeaponManager weaponManager;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        weaponManager = player.GetComponent<PlayerWeaponManager>();

        //PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

        if (PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            PlayerPrefsSaveSystem.SetSaveState(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }

        weaponManager.SwitchWeapon();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Switch Weapon";
        return UIData;
    }
}