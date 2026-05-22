using TMPro;
using UnityEngine;
using System.Collections;


//Made by Michaëla 22-05-2026
public class DialogueUi : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text moreText;
    [SerializeField] private float typingSpeed = 0.03f;
    private bool canPressInput;

    private Dialoge[] currentLines;
    private int currentLine;

    private bool isTyping;
    private bool canContinue;

    private Coroutine typingCoroutine;


    public void StartDialogue(CompletDialogue completDialogue)
    {
        currentLines = completDialogue.lines;
        currentLine = 0;

        canPressInput = false;
        StartCoroutine(EnableInput());

        ShowLine();
    }

    private void Update()
    {
        if (!gameObject.activeSelf)
            return;

        if (canPressInput && Input.GetKeyDown(KeyCode.F)) // change to correct key with interact
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

    void ShowLine()
    {
        canContinue = false;

        UpdateMoreText();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentLines[currentLine].text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;

        dialogueText.text = line;

        // Force TMP to calculate layout first
        dialogueText.ForceMeshUpdate();

        dialogueText.maxVisibleCharacters = 0;

        int totalCharacters = dialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            dialogueText.maxVisibleCharacters = i;

            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
        canContinue = true;
    }

    //if player presses input again auto complete sentance
    void FinishTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.maxVisibleCharacters =
            dialogueText.textInfo.characterCount;

        isTyping = false;
        canContinue = true;

        UpdateMoreText();
    }

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

    void EndDialogue()
    {
        UIManager.Instance.CloseDialogueUI();
    }

    //If here are more lines shows . . . else nothing to indicate to player more dialogue exists
    void UpdateMoreText()
    {
        if (currentLine < currentLines.Length - 1)
        {
            moreText.text = ". . .";
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

