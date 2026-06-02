using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Displays a card inside the inventory/book UI.
///
/// Responsible for:
/// - Showing the card artwork
/// - Displaying card statistics
/// - Showing additional card information when hovered or selected
/// - Coloring the information panel based on card family
/// </summary>
public class InventoryCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // Created by Michaëla 2026-05-13
    //refractered from CardUI
    [SerializeField] public CardData cardData;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image infoboxImage;

    [SerializeField] private GameObject infoBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;

    /// <summary>
    /// Initializes the card UI with the supplied card data.
    /// Assigns the card image and generates the stat description.
    /// </summary>
    /// <param name="data">Card data to display.</param>
    /// <param name="ShowInfo">
    /// Optional parameter for future use when determining
    /// whether card information should be visible by default.
    /// </param>
    public void Setup(CardData data, bool ShowInfo = false)
    {
        cardData = data;

        if (cardImage == null)
            cardImage = GetComponent<Image>();

        if (cardData != null)
        {
            cardImage.sprite = cardData.cardImage;
        }

        CheckStatsForString();
        //cardImage.rectTransform.sizeDelta = new Vector2(300, 100);
    }

    /// <summary>
    /// Builds a formatted string containing all positive stat
    /// modifiers provided by the card and displays it in the UI.
    /// </summary>
    public void CheckStatsForString()
    {
        StringBuilder stats = new StringBuilder();

        if (cardData.healthModifier > 0)
            stats.AppendLine($"Health + {cardData.healthModifier}");

        if (cardData.healRegeneraion > 0)
            stats.AppendLine($"Health regen + {cardData.healRegeneraion}");

        if (cardData.staminaModifier > 0)
            stats.AppendLine($"Stamina + {cardData.staminaModifier}");

        if (cardData.staminaRegeneraion > 0)
            stats.AppendLine($"Stamina regen + {cardData.staminaRegeneraion}");

        if (cardData.luckModifier > 0)
            stats.AppendLine($"Luck + {cardData.luckModifier}%");

        if (cardData.damageModifier > 0)
            stats.AppendLine($"Damage + {cardData.damageModifier * 100}%");

        if (cardData.critChance > 0)
            stats.AppendLine($"Crit chance + {cardData.critChance}%");

        if (cardData.dodgeSpeedModifier > 0)
            stats.AppendLine($"Dodge speed + {cardData.dodgeSpeedModifier}%");

        if (cardData.healModifier > 0)
            stats.AppendLine($"Heal multiplier + {cardData.healModifier}%");

        if (cardData.knockbackModifier > 0)
            stats.AppendLine($"Resistance + {cardData.knockbackModifier}%");

        if (cardData.actionSpeedModifier > 0f)
            stats.AppendLine($"Speed + {cardData.actionSpeedModifier * 100}%");

        if (cardData.weaponSize != Vector3.zero)
            stats.AppendLine($"Weapon size + {cardData.weaponSize.y * 10}");

        statsText.text = stats.ToString();
    }

    /// <summary>
    /// Displays card information when the pointer enters the card.
    /// </summary>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //ToggleInfo();
        ToggleCardInfo();
    }

    /// <summary>
    /// Hides card information when the pointer leaves the card.
    /// </summary>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //ToggleInfo();
        ToggleCardInfo();
    }

    /// <summary>
    /// Displays card information when the UI element is selected.
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        //ToggleInfo();
        ToggleCardInfo();
    }

    /// <summary>
    /// Hides card information when the UI element loses selection.
    /// </summary>
    public void OnDeselect(BaseEventData eventData)
    {
        //ToggleInfo();
        ToggleCardInfo();
    }

    /// <summary>
    /// Toggles the card information panel and updates its contents.
    /// The panel color is determined by the card's family.
    /// </summary>
    public void ToggleInfo()
    {
        if (cardData == null)
            return;

        nameText.text = cardData.cardName;

        if (cardData.cardFamily == CardFamily.Cups)
        {
            ColorUtility.TryParseHtmlString("#5F2828", out var darkRed);
            infoBox.GetComponent<Image>().color = darkRed;
        }
        else if (cardData.cardFamily == CardFamily.Swords)
        {
            ColorUtility.TryParseHtmlString("#4B4A53", out var gray);
            infoBox.GetComponent<Image>().color = gray;
        }
        else if (cardData.cardFamily == CardFamily.Pentacles)
        {
            ColorUtility.TryParseHtmlString("#DAD232", out var yellow);
            infoBox.GetComponent<Image>().color = yellow;
        }
        else if (cardData.cardFamily == CardFamily.Wands)
        {
            ColorUtility.TryParseHtmlString("#435F28", out var green);
            infoBox.GetComponent<Image>().color = green;
        }
        infoBox.SetActive(!infoBox.activeSelf);
    }

    /// <summary>
    /// Toggles the card information panel and updates its contents.
    /// Adds image from card data if available, otherwise uses default styling.
    /// 
    /// Added by Anton 2026-06-01
    /// </summary>
    public void ToggleCardInfo()
    {
        if (cardData == null) 
            return;

        nameText.text = cardData.cardName;

        if (cardData.cardInfoImage != null)
        {
            infoboxImage.sprite = cardData.cardInfoImage;
        }

        infoBox.SetActive(!infoBox.activeSelf);
    }
}
