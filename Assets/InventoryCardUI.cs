using System.Collections;
using System.Text;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCardUI : MonoBehaviour
{
    // Created by Michaëla 2026-05-13
    //refractered from CardUI
    [SerializeField] public CardData cardData;
    [SerializeField] private Image cardImage;

    [SerializeField] private GameObject infoBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;

    private CardUI cardUI;

    public void Awake()
    {
        cardUI = FindFirstObjectByType<CardUI>();
    }
    public void Setup(CardData data, bool ShowInfo = false)
    {
        cardData = data;

        if (cardImage == null)
            cardImage = GetComponent<Image>();

        if (cardData != null)
        {
            cardImage.sprite = cardData.cardImage;
        }
        cardImage.rectTransform.sizeDelta = new Vector2(300, 100);
    }
    void Start()
    {
        
    }
    public void CheckStatsOfCard()
    {
        CheckStatsForString();
    }
    void Update()
    {
        
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

        statsText.text = stats.ToString();
    }



    public void ToggleInfo()
    {
        if (cardData == null)
            return;

        nameText.text = cardData.cardName;
        CheckStatsOfCard();

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
}
