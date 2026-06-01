using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CardShopUI : AutoSelectFirstButtonOnEnable
{
    /// <summary>
    /// Created by Anton 2026-04-28
    /// Initially created as a component to handle the card shop UI, which is opened when interacting with the card shop board.
    /// Also uses the CardSlotShopUI component to populate the card shop slots and handle the selection of cards.
    /// 
    /// Changed by Anton 2026-05-09
    /// Added the linked board slot functionality and added visual effects for hovering and selecting cards.
    /// 
    /// Changed by Anton 2026-05-22
    /// Added changes to purchase button and updating in properly depending on selection of cards.
    /// </summary>

    private PlayerCollection playerCollection;
    private PlayerInput playerInput;
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;
    public ShopBoard shopBoard;
    public InteractCardShop interactCardShop;
    [SerializeField] private BookUi book;

    [Header("Card Information")]
    [field: SerializeField] public List<CardData> purchasedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardSlotShopUI> uiSlots { get; private set; } = new List<CardSlotShopUI>();

    [SerializeField] private int maxPurchased = 1;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button leaveButton;

    [SerializeField] private TextMeshProUGUI errorText;

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;

    protected override void Awake()
    {
        uiSlots.AddRange(GetComponentsInChildren<CardSlotShopUI>(true));

        base.Awake();
    }
    private void Start()
    {
        playerCollection = GameObject.FindWithTag("Player").GetComponent<PlayerCollection>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();

        UpdateUIButtons();
    }
    public void UpdateUIButtons()
    {
        purchaseButton.interactable = purchasedCardData.Count > 0;

        foreach (CardSlotShopUI uiSlot in uiSlots)
        {
            if (uiSlot.CardData == null)
            {
                uiSlot.GetComponent<Button>().interactable = false;
            }
            else
            {
                if (uiSlot.IsSelected)
                {
                    uiSlot.GetComponent<Button>().interactable = false;
                    continue;
                }

                uiSlot.GetComponent<Button>().interactable = true;
            }
        }

        EventSystem.current.SetSelectedGameObject(purchaseButton.gameObject);
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

            UpdateUIButtons();

            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.shopDeselectCardEvent);
            return;
        }

        if (purchasedCardData.Count >= maxPurchased)
        {
            ShowError($"Max {maxPurchased} card!", 2f);
            
            foreach (CardSlotShopUI uiSlot in uiSlots)
            {
                if (uiSlot.IsSelected)
                {
                    uiSlot.SetSelected(false);

                    if (uiSlot.LinkedBoardSlot != null)
                        uiSlot.LinkedBoardSlot.SetSelectedVisual(false);
                }
            }

            purchasedCardData.Clear();
        }

        if (!CanAfford(card))
        {
            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.errorEvent);
            ShowError("Not enough souls!", 2f);

            UpdateUIButtons();

            return;
        }

        if (collection.CardIsPickedUp(card)) // Checks if the card is in equiped lists and temporary lists.
        {
            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.errorEvent);
            ShowError("You already have that card", 2f);

            UpdateUIButtons();
            return;
        }

        slot.SetSelected(true);

        if (slot.LinkedBoardSlot != null)
            slot.LinkedBoardSlot.SetSelectedVisual(true);

        purchasedCardData.Add(card);

        UpdateUIButtons();
    }

    public void OnConfirmSelection()
    {
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();


        foreach (CardData purchasedCard in purchasedCardData)
        {
            if (collection.CardIsPickedUp(purchasedCard)) // Checks if the card is in equiped lists and temporary lists.
            {
                ShowError($"You already have {purchasedCard.cardName}", 2f);
                RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.errorEvent);
                return;
            }
            Event_System.instance.OnSoulsSpent?.Invoke((int)purchasedCard.cardSoulCost);

            CardSlotShopUI uiSlot = uiSlots.Find(slot => slot.CardData == purchasedCard);
            uiSlot?.LinkedBoardSlot?.RemoveCard();
            uiSlot?.SetCard(null);

        }
        Event_System.instance.OnConfirmPurchase?.Invoke(purchasedCardData);

        interactCardShop?.SetBought();

        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.shopBuyCardEvent);
        ResetSlots();
        uiManager.CloseCardShopUI();
        interactCameraHandler.InteractCamReset();

        uiManager.OpenInventoryAfterPurchase();
    }

    public void OnLeaveShop()
    {
        ForceReset();
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
        UpdateUIButtons();
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
        UpdateUIButtons();
    }
}