using FMODUnity;
using UnityEngine;

public class InteractWeaponSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    /// <summary>
    /// Changed by Anton 2026-05-06
    /// Added saved data to the interactable, so that a one time effect can be played the first time the player interacts, and not played again after that.
    /// </summary>

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
        }

        if (firstTimeEffect != null)
            firstTimeEffect.SetActive(false);

        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.weaponSwitchEvent, transform.position);
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