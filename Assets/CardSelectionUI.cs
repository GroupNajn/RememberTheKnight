using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


// Script made by Wilmer some day in april // Henric
// Script Updated by Henric 2026-04-17
// Comments added during -> 2026-04-17 session.
public class CardSelectionUI : AutoSelectFirstButtonOnEnable
{
    private PlayerInput playerInput;
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;

    [SerializeField] private ScrollRect cardScrollRect;
    [SerializeField] private float scrollAmount;

    [SerializeField] private int maxCardsSelected = 4;
    [SerializeField] private TextMeshProUGUI errorText;


    [field: SerializeField] public List<CardData> selectedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardUI> selectedCards { get; private set; } = new List<CardUI>();

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;


    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();

        if (cardScrollRect == null)
            cardScrollRect = uiManager.GetComponentInChildren<ScrollRect>();

        errorText.gameObject.SetActive(false);
        RebuildSelectionState();

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RebuildSelectionState();

    }

    private void OnDisable()
    {
        errorText.gameObject.SetActive(false);
        errorActive = false;
        errorTimer = 0f;
        errorText.alpha = 1f;
    }

    // Method to reset the state of selectes cards. Resets the list, to later check each
    // Button to potentially re-add them to their respective list, since their boolean 
    // is still active inside of the class. 
    private void RebuildSelectionState()
    {
        selectedCards.Clear();
        selectedCardData.Clear();

        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            CardUI card = button.GetComponent<CardUI>();
            if (card == null || card.cardData == null)
                continue;
            SetUnlockable(card.cardData, card);


            if (card.IsSelected && card.IsUnlockable)
            {
                if (selectedCards.Count < maxCardsSelected)
                {
                    selectedCards.Add(card);
                    selectedCardData.Add(card.cardData);
                }
                else
                {
                    card.SetSelected(false);
                }
            }
        }
    }

    private void SetUnlockable(CardData cardData, CardUI cardUI)
    {
        CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        if(cardData == null)
        {
            cardUI.SetUnlockable(false);
            return;
        }

        if (cardSystem.CheckUnlocked(cardData))
        {
            cardUI.SetUnlockable(true);
        }
        else
        {
            cardUI.SetUnlockable(false);
        }
        


    }

    public void OnArrowUp()
    {
        

        cardScrollRect.verticalNormalizedPosition += scrollAmount;
    }

    public void OnArrowDown()
    {
        cardScrollRect.verticalNormalizedPosition -= scrollAmount;
    }

    // If a button is pressed and is active, set to to false, otherwise set it to active.
    // Some if statements inside of the method to check if the selectedCard list is not at its 4 card select limit.
    public void OnCardSelect(Button button)
    {
        CardUI card = button.GetComponent<CardUI>();
        if (card == null || card.cardData == null)
            return;
        

        if (card.IsSelected && card.IsUnlockable)
        {
            card.SetSelected(false);
            selectedCards.Remove(card);
            selectedCardData.Remove(card.cardData);
            Debug.Log("Card Deselected");
            return;
        }
        if (!card.IsUnlockable)
        {
            if (!errorActive)
                ShowError($"This card is not unlocked!" ,3f);
            return ;
        }

        if (selectedCards.Count >= maxCardsSelected)
        {
            if (!errorActive)
                ShowError($"You can only select {maxCardsSelected} cards!", 3f);

            return;
        }

        card.SetSelected(true);
        selectedCards.Add(card);
        selectedCardData.Add(card.cardData);
    }

    // Confirm the selected cards and send a delegate event to the Event_System of which selectes cards
    // With the cardData list as a parameter. 
    public void OnConfirmSelection()
    {
        Event_System.instance.OnStatsApplied?.Invoke(selectedCardData);
        uiManager.CloseCardSelectUI();
        interactCameraHandler.InteractCamReset();
    }

    private void Update()
    {
        if (!errorActive) return;

        errorTimer -= Time.unscaledDeltaTime;

        if (errorTimer <= fadeDuration)
        {
            float alpha = Mathf.Clamp01(errorTimer / fadeDuration);
            errorText.alpha = alpha;
        }


        if (errorTimer <= 0f)
        {
            errorText.gameObject.SetActive(false);
            errorActive = false;
            errorText.alpha = 1f;
        }
    }

    private void ShowError(string message, float duration)
    {
        errorText.text = message;
        fadeDuration = duration * 0.25f;
        errorText.alpha = 1f;
        errorText.gameObject.SetActive(true);
        errorActive = true;
        errorTimer = duration;
    }
}