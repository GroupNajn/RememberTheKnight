using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BookPageUI : MonoBehaviour
{
    //made by Michaëla 2026-04-19

    /// <summary>
    /// Changed by Anton 2026-05-16
    /// Overhauled and refactored some code
    /// 
    /// Reworked by Anton 2026-05-26
    /// Added more stats pages to show more stats.
    /// </summary>

    [Header("Panels")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private GameObject lorePanel;

    [Header("Stats")]
    [SerializeField] private bool IsPlayerStats;
    [SerializeField] private TextMeshProUGUI statsTextTopLeft;
    [SerializeField] private TextMeshProUGUI statsTextTopRight;
    [SerializeField] private TextMeshProUGUI statsTextBottomLeft;
    [SerializeField] private TextMeshProUGUI statsTextBottomRight;

    [Header("Cards")]
    [SerializeField] private Transform cardContainer;
    [SerializeField] private InventoryCardUI cardPrefab;

    [Header("Lore")]
    [SerializeField] private TextMeshProUGUI loreTitleText;
    [SerializeField] private TextMeshProUGUI loreText;

    private PlayerCollection playerCollection;

    public void Setup(PageData data)
    {
        ClearPages();

        // Then enable the relevant panel based on the page type
        switch (data.type)
        {
            case PageData.PageType.Stats:
                statsPanel.SetActive(true);
                ShowStats(data.stats, data.weaponStats);
                break;

            case PageData.PageType.Cards:
                cardPanel.SetActive(true);
                ShowCards(data.cards);
                break;

            case PageData.PageType.Lore:
                lorePanel.SetActive(true);
                
                loreTitleText.text = string.IsNullOrEmpty(data.loreTitle) ? "" : data.loreTitle;
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

    private void ShowStats(PlayerStats stats, PlayerWeaponManager weaponStats)
    {
        if (stats == null) return;

        playerCollection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>();

        if (IsPlayerStats)
        {
            statsTextTopLeft.text =
            $"-HEALTH-\n" +
            $"Health: {stats.MaxHealth}\n" +
            $"Resistance: {stats.currentKnockbackResistance}%\n";
            //$"Health Regen: {stats.healthRegen}\n" +
            //$"Total Heal: {stats.TotalHeal}\n" +

            statsTextTopRight.text =
            $"-STAMINA-\n" +
            $"Stamina: {stats.maxStamina}\n" +
            $"Stamina Regen: {stats.staminaRegen}\n" +
            $"Speed: {stats.currentActionSpeedModifier}\n" +
            $"Speed bonus: {(stats.currentActionSpeedModifier - stats.baseActionSpeed) * 100}%\n";

            statsTextBottomLeft.text =
            $"-DAMAGE-\n" +
            $"Light Damage: {weaponStats.currentActiveWeaponData.LightDamage}\n" +
            $"Heavy Damage: {weaponStats.currentActiveWeaponData.HeavyDamage}\n" +
            $"Damage bonus: {(stats.currentDamageModifier - 1) * 100}%\n";

            statsTextBottomRight.text =
            $"-CHANCE-\n" +
            $"Luck: {stats.currentLuck}%\n" +
            $"Crit chance: {stats.currentCritChance}%\n";
        }
        else
        {
            statsTextTopLeft.text =
            $"-FAMILY-\n" +
            $"Family: {(playerCollection.playerContract != null ? playerCollection.playerContract.CardFamily.ToString() : "None")}\n";

            statsTextTopRight.text =
            $"-PROGRESSION-\n" +
            $"Level: {RunGameData.Instance.LevelCounter} \n";

            statsTextBottomLeft.text =
            $"-COMBAT-\n" +
            $"Enemies Slain: \n" +
            $"Bosses Slain: \n";

            statsTextBottomRight.text =
            $"-LOOT-\n" +
            $"Souls Collected: \n" +
            $"Souls Sacrificed: \n" +
            $"Cards Collected: \n";
        }
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

            // You can't modify CardUI, assign directly
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


