using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractableLore : MonoBehaviour, IInteractable, IInteractableUIText
{
    // created by Anton
    
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
    }

    public void Interact()
    {
        if (!PlayerPrefsSaveSystem.HasInteracted(loreEntry.id))
        {
            PlayerPrefsSaveSystem.SetSaveState(loreEntry.id);
        }

        loreManager.UnlockLore(loreEntry.id);
        lorePageUI.SetLoreEntry(loreEntry);
        playerUIManager.OpenLorePageUI();

    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Unveil Lore";
        return UIData;
    }
}
