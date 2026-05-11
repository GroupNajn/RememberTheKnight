using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlotShopUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI soulCostText;

    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlockable { get; private set; } = false;
    [SerializeField] public bool isLocked;


    [SerializeField] private ShopBoardCardSlot linkedBoardSlot;
    public ShopBoardCardSlot LinkedBoardSlot => linkedBoardSlot;

    [SerializeField] private CardData cardData;
    public CardData CardData => cardData;

    void Awake()
    {
        IsSelected = false;

    }
    void Start()
    {
        IsSelected = false;
    }
   
    public void SetCard(CardData card)
    {
        if (isLocked)
        {
            cardData = null;
        }

        if (card == null)
            return;

        cardData = card;
        soulCostText.text = card.cardSoulCost.ToString();
    }
    public void SetBoardSlot(ShopBoardCardSlot slot)
    {
        linkedBoardSlot = slot;
    }
    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public void SetUnlockable(bool unlockable)
    {
        this.IsUnlockable = unlockable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (linkedBoardSlot != null)
            linkedBoardSlot.SetHoverVisual(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (linkedBoardSlot != null)
            linkedBoardSlot.SetHoverVisual(false);
    }
}