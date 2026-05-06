using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardShopUI : AutoSelectFirstButtonOnEnable
{
    private PlayerCollection playerCollection;
    private PlayerInput playerInput;
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;
    public ShopBoard shopBoard;

    [SerializeField] private TextMeshProUGUI errorText;

    [field: SerializeField] public List<CardData> purchasedCardData { get; private set; } = new List<CardData>();
    [field: SerializeField] public List<CardSlotShopUI> uiSlots { get; private set; } = new List<CardSlotShopUI>();
    [SerializeField] private int maxPurchased = 1;

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    private bool errorActive = false;

    void Awake()
    {
        uiSlots.AddRange(GetComponentsInChildren<CardSlotShopUI>(true));
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

        if (slot.IsSelected)
        {
            slot.SetSelected(false);
            purchasedCardData.Remove(card);
            return;
        }

        if (purchasedCardData.Count >= maxPurchased)
        {
            ShowError($"Max {maxPurchased} cards!", 2f);
            return;
        }

        if (!CanAfford(card))
        {
            ShowError("Not enough souls!", 2f);
            return;
        }

        slot.SetSelected(true);
        purchasedCardData.Add(card);
        Debug.Log($"Selected card: {card.name}");
    }

    public void OnConfirmSelection()
    {
        Event_System.instance.OnStatsApplied?.Invoke(purchasedCardData);
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
}