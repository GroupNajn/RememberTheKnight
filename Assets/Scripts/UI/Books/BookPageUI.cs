using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class BookPageUI : MonoBehaviour
{


    [Header("Panels")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject textPanel;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI statsText;

    [Header("Cards")]
    [SerializeField] private Transform cardContainer;
    [SerializeField] private CardUI cardPrefab;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI pageText;

    public void Setup(PageData data)
    {
        // Disable all panels first
        statsPanel.SetActive(false);
        cardPanel.SetActive(false);
        textPanel.SetActive(false);

        switch (data.type)
        {
            case PageData.PageType.Stats:
                statsPanel.SetActive(true);
                ShowStats(data.stats);
                break;

            case PageData.PageType.Cards:
                cardPanel.SetActive(true);
                ShowCards(data.cards);
                break;

            case PageData.PageType.Text:
                textPanel.SetActive(true);
                pageText.text = data.text;
                break;
        }
    }

    private void ShowStats(PlayerStats stats)
    {
        if (stats == null) return;

        statsText.text =
            $"HP: \n" +
            $"Damage: \n" +
            $"Mana: ";
    }

    private void ShowCards(List<CardData> cards)
    {
        // Clear old cards
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        // Add new cards
        foreach (var card in cards)
        {
            var ui = Instantiate(cardPrefab, cardContainer);
           // ui.SetData(card);
        }
    }
}
