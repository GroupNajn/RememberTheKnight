using UnityEngine;

public class InteractFamlySelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    UIManager playerUIManager;

    private bool canShowUI = false;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();

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

        playerUIManager.OpenFamilySelectUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "[F]: Choose Your Faith";
        return UIData;
    }
}