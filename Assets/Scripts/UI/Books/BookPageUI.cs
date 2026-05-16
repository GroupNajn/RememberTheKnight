using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BookPageUI : MonoBehaviour
{
    //made by Michaëla 2026-04-19

    [Header("Panels")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject lorePanel;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI statsText;

    [Header("Cards")]
    [SerializeField] private Transform cardContainer;
    [SerializeField] private InventoryCardUI cardPrefab;

    [Header("Lore")]
    [SerializeField] private TextMeshProUGUI loreText;

    public void Setup(PageData data)
    {
        ClearPages();

        // Then enable the relevant panel based on the page type
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

            case PageData.PageType.Lore:
                lorePanel.SetActive(true);

                loreText.text = string.IsNullOrEmpty(data.loreText) ? "" : data.loreText;

                break;
        }
    }

    public void ClearPages()
    {
        statsPanel.SetActive(false);
        cardPanel.SetActive(false);
        lorePanel.SetActive(false);
    }

    private void ShowStats(PlayerStats stats)
    {
        if (stats == null) return;
        //LightDamage, heavyDamage, CombodamageModifier
        // MaxHP, HealthRegen, TotalHeal
        //Stamina, StaminaRegen 
        //Luck , CritRate

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
          $"Crit Rate: {stats.currentCritChance}\n";
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
            ui.transform.SetAsLastSibling();
            ui.Setup(card, true);

            //Button button = ui.GetComponent<Button>();

            //if (button != null)
            //{
            //    button.onClick.RemoveAllListeners();
            //    button.onClick.AddListener(ui.ToggleInfo);
            //}
          
        }
    }

}


