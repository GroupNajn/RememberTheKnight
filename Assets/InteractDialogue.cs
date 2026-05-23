using UnityEngine;


//Made by Michaëla 22-05-2026
public class InteractDialogue : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID; 
    [SerializeField] private GameObject firstTimeEffect;

    [Header("Dialogue")]
    [SerializeField] Dialogue[] dialogueLines;

    private UIManager playerUIManager; 

    void Start() 
    {
        playerUIManager = FindFirstObjectByType<UIManager>(); 
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
        playerUIManager.OpenDialogueUI(dialogueLines);
    }

    public InteractableUIData GetUIData() 
    {
        var UIData = new InteractableUIData();
        UIData.InfoText = "Speaketh to The lady of the squeer";
        return UIData; 
    } 
}
