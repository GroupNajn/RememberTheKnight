using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractableLore : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    [Header("Lore")]
    [SerializeField] private LoreEntry loreEntry;
    private LoreManager loreManager;

    private UIManager playerUIManager;


    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        //interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
        //stateDrivenCamera = GameObject.FindWithTag("StateDrivenCamera").GetComponent<CinemachineStateDrivenCamera>();
        loreManager = FindFirstObjectByType<LoreManager>();

        PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

        if (InteractableSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!InteractableSaveSystem.HasInteracted(interactableID))
        {
            InteractableSaveSystem.SetInteracted(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }

        loreManager.UnlockLore(loreEntry.id);
        playerUIManager.OpenPedistalBook(loreEntry);
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "[F]: Unveil Lore";
        return UIData;
    }
}
