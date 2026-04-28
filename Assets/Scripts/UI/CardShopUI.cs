using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardShopUI : MonoBehaviour
{
    private PlayerInput playerInput;
    private UIManager uiManager;

    [SerializeField] private TextMeshProUGUI errorText;

    private float cardCost;
    [field: SerializeField] public List<CardData> selectedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardUI> selectedCards { get; private set; } = new List<CardUI>();

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();

        uiManager.CloseCardSelectUI();
    }

    // If a button is pressed and is active, set to to false, otherwise set it to active.
    // Some if statements inside of the method to check if the selectedCard list is not at its 4 card select limit.
    public void OnCardSelect(Button button)
    {
        CardUI card = button.GetComponent<CardUI>();
        if (card == null || card.cardData == null)
            return;

        //if (card.IsSelected)
        //{
        //    card.SetSelected(false);
        //    selectedCards.Remove(card);
        //    selectedCardData.Remove(card.cardData);
        //    Debug.Log("Card Deselected");
        //    return;
        //}

        //if (selectedCards.Count > maxCardsSelected)
        //{
        //    if (!errorActive)
        //        ShowError($"You can only select {maxCardsSelected} cards!", 5f);

        //    return;
        //}

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
