using UnityEngine;
using System.Collections.Generic;


//Made by Michaëla 22-05-2026
public class InteractDialogue : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID; 
    [SerializeField] private GameObject firstTimeEffect;

    [Header("Interactable Objects")]
    [SerializeField] private List<GameObject> objectsToActivateOnInteract;

    [Header("Dialogue")]
    [SerializeField] Dialogue[] customDialogue;
    [SerializeField] CompleteDialogue presetDialogue;

    private UIManager playerUIManager; 

    void Start() 
    {
        playerUIManager = FindFirstObjectByType<UIManager>();

        if (PlayerPrefsSaveSystem.HasInteracted(interactableID)) 
        {
            if (firstTimeEffect != null) 
                firstTimeEffect.SetActive(false);
        }
    }

    public Dialogue[] GetDialogue()
    {
        if (presetDialogue != null)
            return presetDialogue.lines;

        return customDialogue;
    }
    public void Interact() 
    {
        if (!PlayerPrefsSaveSystem.HasInteracted(interactableID)) 
        {
            PlayerPrefsSaveSystem.SetSaveState(interactableID); 
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);

            foreach (var obj in objectsToActivateOnInteract)
            {
                obj.SetActive(true);
            }
        }
        playerUIManager.OpenDialogueUI(GetDialogue());
    }

    public InteractableUIData GetUIData() 
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "The Old Lady";
        return UIData; 
    } 
}
