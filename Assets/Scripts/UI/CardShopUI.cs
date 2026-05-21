using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardShopUI : AutoSelectFirstButtonOnEnable
{
    private PlayerCollection playerCollection;
    private PlayerInput playerInput;
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;
    public ShopBoard shopBoard;
    public InteractCardShop interactCardShop;

    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Card Information")]
    [field: SerializeField] public List<CardData> purchasedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardSlotShopUI> uiSlots { get; private set; } = new List<CardSlotShopUI>();

    [SerializeField] private int maxPurchased = 1;

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;

    void Awake()
    {
        uiSlots.AddRange(GetComponentsInChildren<CardSlotShopUI>(true));

        base.Awake();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void Start()
    {
        playerCollection = GameObject.FindWithTag("Player").GetComponent<PlayerCollection>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
    }

    public void PopulateSlots()
    {
        var cards = shopBoard.Slots;

        if (cards == null || cards.Count == 0)
            return;

        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < cards.Count)
            {
                uiSlots[i].SetCard(cards[i].CurrentCard);
                uiSlots[i].SetBoardSlot(cards[i]);
            }
            else
            {
                uiSlots[i].SetCard(null);
            }
        }
    }

    private bool CanAfford(CardData card)
    {
        float currentSoulCollect = LootManager.instance.GetComponent<Loot_System>().currentSoulCount;

        return (currentSoulCollect >= card.cardSoulCost);
    }

    public void OnCardSelect(CardSlotShopUI slot)
    {
        if (slot == null || slot.CardData == null)
            return;

        CardData card = slot.CardData;
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();

        if (slot.IsSelected)
        {
            slot.SetSelected(false);

            if (slot.LinkedBoardSlot != null)
                slot.LinkedBoardSlot.SetSelectedVisual(false);

            purchasedCardData.Remove(card);
            return;
        }

        if (purchasedCardData.Count >= maxPurchased)
        {
            ShowError($"Max {maxPurchased} cards!", 2f);
            
            foreach (CardSlotShopUI uiSlot in uiSlots)
            {
                if (uiSlot.IsSelected)
                {
                    uiSlot.SetSelected(false);

                    if (uiSlot.LinkedBoardSlot != null)
                        uiSlot.LinkedBoardSlot.SetSelectedVisual(false);
                }
            }

            purchasedCardData.Remove(card);
        }

        if (!CanAfford(card))
        {
            ShowError("Not enough souls!", 2f);
            return;
        }

        if (collection.CardIsPickedUp(card)) // Checks if the card is in equiped lists and temporary lists.
        {
            ShowError("You already have that card", 2f);
            return;
        }

        slot.SetSelected(true);

        if (slot.LinkedBoardSlot != null)
            slot.LinkedBoardSlot.SetSelectedVisual(true);

        purchasedCardData.Add(card);
    }

    public void OnConfirmSelection()
    {
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();


        foreach (CardData purchasedCard in purchasedCardData)
        {
            if (collection.CardIsPickedUp(purchasedCard)) // Checks if the card is in equiped lists and temporary lists.
            {
                ShowError($"You already have {purchasedCard.cardName}", 2f);
                return;
            }
            Event_System.instance.OnSoulsSpent?.Invoke((int)purchasedCard.cardSoulCost);

            //CardSlotShopUI uiSlot = uiSlots.Find(slot => slot.CardData == purchasedCard);
            //uiSlot?.LinkedBoardSlot?.RemoveCard();

        }
        Event_System.instance.OnConfirmPurchase?.Invoke(purchasedCardData);

        interactCardShop?.SetBought();

        ResetSlots();
        uiManager.CloseCardShopUI();
        interactCameraHandler.InteractCamReset();
    }

    public void OnLeaveShop()
    {
        ResetSlots();
        uiManager.CloseCardShopUI();
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

    private void ResetSlots()
    {
        PlayerCollection collecton = GameObject.Find("Player").GetComponent<PlayerCollection>();

        foreach (CardSlotShopUI uislot in uiSlots)
        {
            if (collecton.CardIsPickedUp(uislot.CardData))
            {
                uislot.SetSelected(false);
                uislot.cardInfo.transform.localScale = Vector3.zero;

                if (uislot.LinkedBoardSlot != null)
                {
                    uislot.LinkedBoardSlot.SetHoverVisual(false);
                    uislot.LinkedBoardSlot.SetSelectedVisual(false);
                }
            }
        }
        purchasedCardData.Clear();
    }

    public void ForceReset()
    {
        foreach (CardSlotShopUI uislot in uiSlots)
        {
            uislot.SetSelected(false);
            uislot.cardInfo.transform.localScale = Vector3.zero;

            if (uislot.LinkedBoardSlot != null)
            {
                uislot.LinkedBoardSlot.SetHoverVisual(false);
                uislot.LinkedBoardSlot.SetSelectedVisual(false);
            }
        }
        purchasedCardData.Clear();
    }

}