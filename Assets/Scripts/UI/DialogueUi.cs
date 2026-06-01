using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles dialogue presentation including:
/// - Typewriter text animation
/// - Dialogue progression
/// - Input handling
/// - Dialogue sound effects
/// </summary>

//Made by Michaëla 22-05-2026
public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text moreText;
    [SerializeField] private float typingSpeed = 0.03f;
    private bool canPressInput;

    private Dialogue[] currentLines;
    private int currentLine;

    private bool isTyping;
    private bool canContinue;

    private Coroutine typingCoroutine;
    private EventInstance talkingInstance;
    
    string interactKey;
    [SerializeField] private InputActionReference interactAction;

    private void OnEnable()
    {
        interactKey = InputManager.Instance.SetInteractBinding();

        if (interactAction != null && interactAction.action != null)
            interactAction.action.Enable();
    }

    private void Awake()
    {
        talkingInstance = RuntimeManager.CreateInstance(WorldSoundFXManager.instance.ladyTalkingEvent);
    }

    /// <summary>
    /// Begins a dialogue sequence.
    /// </summary>
    /// <param name="dialogueLines">Dialogue lines to display.</param>
    public void StartDialogue(Dialogue[] dialogueLines)
    {
        currentLines = dialogueLines;
        currentLine = 0;

        canPressInput = false;
        StartCoroutine(EnableInput());

        ShowLine();
    }

    private void Update()
    {
        interactKey = InputManager.Instance.SetInteractBinding();

        if (!gameObject.activeSelf)
            return;

        if (canPressInput && interactAction.action.WasPressedThisFrame()) // change to correct key with interact
        {
            if (isTyping)
            {
                FinishTyping();
            }
            else if (canContinue)
            {
                NextLine();
            }
        }

    }

    /// <summary>
    /// Displays the current dialogue line and starts the typing effect.
    /// </summary>
    void ShowLine()
    {
        canContinue = false;

        UpdateMoreText();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            talkingInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        typingCoroutine = StartCoroutine(TypeLine(currentLines[currentLine].text));
    }

    /// <summary>
    /// Reveals dialogue characters over time,
    /// creating a typewriter effect.
    /// </summary>
    /// <param name="line">Text to display.</param>
    IEnumerator TypeLine(string line)
    {
        isTyping = true;

        dialogueText.text = line;

        // Force TMP to calculate layout first
        dialogueText.ForceMeshUpdate();

        dialogueText.maxVisibleCharacters = 0;

        talkingInstance.start();

        int totalCharacters = dialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        talkingInstance.stop(STOP_MODE.ALLOWFADEOUT);

        isTyping = false;
        canContinue = true;
    }

    /// <summary>
    /// Immediately reveals the remainder of the current dialogue line.
    /// </summary>
    void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.maxVisibleCharacters =
            dialogueText.textInfo.characterCount;

        talkingInstance.stop(STOP_MODE.ALLOWFADEOUT);


        isTyping = false;
        canContinue = true;

        UpdateMoreText();
    }

    /// <summary>
    /// Advances to the next dialogue line
    /// or ends the dialogue if no lines remain.
    /// </summary>
    void NextLine()
    {
        currentLine++;

        if (currentLine >= currentLines.Length)
        {
            
            EndDialogue();
            return;
        }
        UpdateMoreText();
        ShowLine();
    }

    /// <summary>
    /// Closes the dialogue UI and ends the conversation.
    /// </summary>
    void EndDialogue()
    {
        UIManager.Instance.CloseDialogueUI();
    }
    /// <summary>
    /// If here are more lines shows . . . else nothing to indicate to player more dialogue exists
    /// </summary>
    void UpdateMoreText()
    {

        if (currentLine < currentLines.Length - 1)
        {
            moreText.text = $". . . [{interactKey}]";
        }
        else
        {
            moreText.text = "";
        }
    }
    IEnumerator EnableInput()
    {
        yield return null;
        canPressInput = true;
    }
}

