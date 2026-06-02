using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractableLore : MonoBehaviour, IInteractable, IInteractableUIText
{
<<<<<<< Updated upstream
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;
=======
    // created by Anton
>>>>>>> Stashed changes

    [Header("Lore")]
    [SerializeField] private LoreEntry loreEntry;
    [SerializeField] private LorePageUI lorePageUI;
    private LoreManager loreManager;

    private UIManager playerUIManager;


    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        loreManager = FindFirstObjectByType<LoreManager>();
        lorePageUI = FindFirstObjectByType<LorePageUI>(FindObjectsInactive.Include);

<<<<<<< Updated upstream
        PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

        if (InteractableSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
=======
        //PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress
>>>>>>> Stashed changes
    }

    public void Interact()
    {
<<<<<<< Updated upstream
        if (!InteractableSaveSystem.HasInteracted(interactableID))
        {
            InteractableSaveSystem.SetInteracted(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
=======
        if (!PlayerPrefsSaveSystem.HasInteracted(loreEntry.id))
        {
            PlayerPrefsSaveSystem.SetSaveState(loreEntry.id);
>>>>>>> Stashed changes
        }

        loreManager.UnlockLore(loreEntry.id);
        lorePageUI.SetLoreEntry(loreEntry);
        playerUIManager.OpenLorePageUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "Unveil Lore";
        return UIData;
    }
}
