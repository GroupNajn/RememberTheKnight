using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSlotShopUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("CardData information")]
    [SerializeField] public GameObject cardInfo;
    [SerializeField] private Image cardInfoImage;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardStats;

    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] GameObject soulCostDisplay;

    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlockable { get; private set; } = false;
    [SerializeField] public bool isLocked;


    [SerializeField] private ShopBoardCardSlot linkedBoardSlot;
    public ShopBoardCardSlot LinkedBoardSlot => linkedBoardSlot;

    [SerializeField] private CardData cardData;
    public CardData CardData => cardData;

    [Header("Animation")]
    [SerializeField] private float sizeStart = 0f;
    [SerializeField] private float sizeEnd = 1f;


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

        soulCostText.text = card.cardSoulCost.ToString();
    }

    IEnumerator CardInfoShowcase(bool selected)
    {
        yield return new WaitForSeconds(LinkedBoardSlot.AnimationDuration);

        cardInfoImage = cardInfo.GetComponent<Image>();
        cardName = cardInfo.GetComponentInChildren<TextMeshProUGUI>();
        cardStats = cardInfo.GetComponentsInChildren<TextMeshProUGUI>()[1];

        cardInfoImage.sprite = cardData.cardInfoImage;
        cardName.text = cardData.cardName;
        CheckStatsForString();

        AnimateSelectSize(selected);
    }
    public void SetBoardSlot(ShopBoardCardSlot slot)
    {
        linkedBoardSlot = slot;
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        linkedBoardSlot?.SetSelectedVisual(selected);

        if (cardData == null)
            return;

        StartCoroutine(CardInfoShowcase(selected));
    }

    public void SetUnlockable(bool unlockable)
    {
        this.IsUnlockable = unlockable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardData == null)
            return;

        linkedBoardSlot?.SetHoverVisual(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardData == null)
            return;

        linkedBoardSlot?.SetHoverVisual(false);
    }

    public void CheckStatsForString()
    {
        StringBuilder stats = new StringBuilder();

        if (cardData.healthModifier > 0)
            stats.AppendLine($"Health + {cardData.healthModifier}");

        if (cardData.staminaModifier > 0)
            stats.AppendLine($"Stamina + {cardData.staminaModifier}");

        if (cardData.luckModifier > 0)
            stats.AppendLine($"Luck + {cardData.luckModifier}%");

        if (cardData.damageModifier > 0)
            stats.AppendLine($"Damage + {cardData.damageModifier * 100}%");

        if (cardData.critChance > 0)
            stats.AppendLine($"critical chance + {cardData.critChance}%");

        if (cardData.walkSpeedModifier > 0)
            stats.AppendLine($"walk speed + {cardData.walkSpeedModifier}");

        if (cardData.sprintSpeedModifier > 0)
            stats.AppendLine($"sprint speed + {cardData.sprintSpeedModifier}%");

        if (cardData.dodgeSpeedModifier > 0)
            stats.AppendLine($"dodge speed + {cardData.dodgeSpeedModifier}%");

        if (cardData.healModifier > 0)
            stats.AppendLine($" heal multiplier + {cardData.healModifier}%");

        if (cardData.knockbackModifier > 0)
            stats.AppendLine($"resistance + {cardData.knockbackModifier}%");

        if (cardData.weaponSize != Vector3.zero)
            stats.AppendLine($"weapon size + {cardData.weaponSize.y * 10}");

        cardStats.text = stats.ToString();
    }

    // Animations for sizing the information when selected
    public void AnimateSelectSize(bool selected)
    {
        float targetSize = selected ? sizeEnd : sizeStart;
        LeanTween.scale(cardInfo, Vector3.one * targetSize, linkedBoardSlot.AnimationDuration).setEase(LeanTweenType.easeInOutQuad);
    }
}