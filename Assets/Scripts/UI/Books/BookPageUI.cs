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
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private CardSelectionUI cardSelectionUI;
    [SerializeField] private bool useTestCards = true;

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

            /*case PageData.PageType.Cards:
                cardPanel.SetActive(true);
                if (data.cards == null || data.cards.Count == 0)
                {
                    data.cards = new List<CardData>();

                    data.cards.AddRange(playerCollection.ReturnPermanentCardCollection());
                    data.cards.AddRange(playerCollection.ReturnTempCardCollection());
                }

                ShowCards(data.cards);
                break;
            */
            case PageData.PageType.Cards:
                cardPanel.SetActive(true);

                if (data.cards != null)
                    ShowCardsTemp(data.cards);

                break;

            case PageData.PageType.Text:
                textPanel.SetActive(true);

                if (data.text != null)
                    pageText.text = data.text;
                else
                    pageText.text = ""; // or "No text"

                break;
        }
    }

    private void ShowStats(PlayerStats stats)
    {
        if (stats == null) return;
        //LightDamage, heavyDamage, CombodamageModifier
        // MaxHP, HealthRegen, TotalHeal
        //Stamina, StaminaRegen 
        //Luck , CritRate
        //dodgeCoolCown



        statsText.text =
          // $"Light Damage: {stats.LightDamage}\n" +
          // $"Heavy Damage: {stats.HeavyDamage}\n" +
          // $"Combo Damage Modifier: {stats.ComboDamageModifier}";
          $"Max HP: {stats.MaxHealth}\n" +
          $"Health Regen: {stats.healthRegenRate}\n" +
          //$"Total Heal: {stats.TotalHeal}\n" +
          $"Stamina: {stats.maxStamina}\n" +
          $"Stamina Regen: {stats.staminaRegenRate}\n" +
          $"Luck: {stats.currentLuck}\n" +
          $"Crit Rate: {stats.currentCritChance}\n" +
          $"Dodge Cooldown: {stats.dodgeCoolDown}\n";
         
    }

   
    
        private void ShowCards(List<CardData> cards)
    {
        // Safety check
        if (cards == null)
        {
            Debug.LogWarning("ShowCards called with null list!");
            return;
        }

        // Clear old cards
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        // Spawn new cards
        foreach (var card in cards)
        {
            var ui = Instantiate(cardPrefab, cardContainer);

            // You can't modify CardUI → assign directly
            ui.cardData = card;
        }
    }

    private void ShowCardsTemp(List<CardData> cards)
    {
        if (cards == null) return;

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (var card in cards)
        {
            var ui = Instantiate(cardPrefab, cardContainer);
            ui.cardData = card;
        }
    }
}


