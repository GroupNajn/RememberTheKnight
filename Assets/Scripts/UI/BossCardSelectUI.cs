using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BossCardSelectUI : AutoSelectFirstButtonOnEnable
{
    private UIManager uiManager;

    [SerializeField] private int maxCardsSelected = 1;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] public BossCardUI CurrentSelectedCard {  get; private set; }
    [field: SerializeField] public List<CardData> selectedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<BossCardUI> selectedCards { get; private set; } = new List<BossCardUI>();

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;


    private void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();

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
            BossCardUI card = button.GetComponent<BossCardUI>();
            if (card == null || card.cardData == null)
                continue;

            SetUnlockedCards(card.cardData, card);


            if (card.IsSelected && card.IsUnlocked)
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

    private void SetUnlockedCards(CardData cardData, BossCardUI bossCardUI)
    {
        CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        if (cardData == null)
        {
            bossCardUI.SetUnlocked(false);
            return;
        }

        if (cardSystem.CheckUnlocked(cardData))
        {
            bossCardUI.SetUnlocked(true);
        }
        else
        {
            bossCardUI.SetUnlocked(false);
        }

    }

    // If a button is pressed and is active, set to to false, otherwise set it to active.
    // Some if statements inside of the method to check if the selectedCard list is not at its 4 card select limit.
    public void OnCardSelect(Button button)
    {
        BossCardUI card = button.GetComponent<BossCardUI>();
        if (card == null || card.cardData == null)
            return;


        if (card.IsSelected && card.IsUnlocked)
        {
            card.SetSelected(false);

            if (CurrentSelectedCard == card)
                CurrentSelectedCard = null;

            selectedCards.Remove(card);
            selectedCardData.Remove(card.cardData);
            return;
        }
        if (!card.IsUnlocked)
        {
            if (!errorActive)
                ShowError($"This card is not unlocked!", 3f);
            return;
        }

        if (selectedCards.Count >= maxCardsSelected)
        {
            if (!errorActive)
                ShowError($"You can only select {maxCardsSelected} cards!", 3f);

            return;
        }

        if (selectedCards.Count >= maxCardsSelected)
        {
            selectedCards[0].SetSelected(false);
            selectedCardData.Remove(selectedCards[0].cardData);
            selectedCards.Clear();
        }

        card.SetSelected(true);

        CurrentSelectedCard = card;

        selectedCards.Add(card);
        selectedCardData.Add(card.cardData);
    }

    // Confirm the selected cards and send a delegate event to the Event_System of which selectes cards
    // With the cardData list as a parameter. 
    public void OnConfirmSelection()
    {
        Event_System.instance.OnConfirmCardSelection?.Invoke(selectedCardData);
        uiManager.CloseBossCardSelectUI();
    }
    public void OnExit()
    {
        if (CurrentSelectedCard != null)
        {
            CurrentSelectedCard.SetSelected(false);
            CurrentSelectedCard = null;
        }

        selectedCards.Clear();
        selectedCardData.Clear();

        uiManager.CloseBossCardSelectUI();
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
