using UnityEngine;
using System.Collections.Generic;


//Made by Michaëla 22-05-2026
public class InteractDialogue : MonoBehaviour, IInteractable, IInteractableUIText
{

    /// <summary>
    /// Changed by Anton 2026-05-23
    /// Added logic to use either custom dialogue or preset dialogue.
    /// 
    /// Changed by Anton 2026-05-25
    /// Added objects to active on interact, this is used to activate indicators on other objects after dialogue.
    /// </summary>
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

    /// <summary>
    /// Retrieves an array of dialogue lines from either the preset or custom dialogue source.
    /// </summary>
    /// <returns>An array of Dialogue objects containing either preset or custom dialogue lines.</returns>
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

            if (objectsToActivateOnInteract != null)
            {
                foreach (var obj in objectsToActivateOnInteract)
                {
                    obj.SetActive(true);
                }
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
