using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlotShopUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("CardData information")]
    [SerializeField] private GameObject cardInfo;
    [SerializeField] private Image cardInfoImage;

    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] GameObject soulCostDisplay;

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
        {
            soulCostDisplay.SetActive(false);
            return;
        }

        cardData = card;
        cardInfoImage = cardInfo.GetComponent<Image>();
        cardInfoImage.sprite = cardData.cardInfoImage;
        soulCostText.text = card.cardSoulCost.ToString();
    }
    public void SetBoardSlot(ShopBoardCardSlot slot)
    {
        linkedBoardSlot = slot;
    }
    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        cardInfo.SetActive(selected);

        if (linkedBoardSlot != null)
            linkedBoardSlot.SetSelectedVisual(selected);
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