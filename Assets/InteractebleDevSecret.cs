using UnityEngine;

public class InteractebleDevSecret : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    [Header("Lore")]
    [SerializeField] private DevPageUI devPageUI;

    private UIManager playerUIManager;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();

        PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

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

        playerUIManager.OpenDevSecretUI();
    }
    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Read letter";
        return UIData;
    }
}
