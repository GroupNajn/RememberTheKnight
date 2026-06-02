using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the visual presentation of a single book page.
/// Supports displaying:
/// - Player/Run statistics
/// - Card collections
/// - Lore entries
///
/// The page type is determined by the supplied PageData object.
/// </summary>
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
    private GameData gameData;

    /// <summary>
    /// Configures the page based on the supplied PageData.
    /// Activates the correct panel and populates it with content.
    /// </summary>
    /// <param name="data">Page information to display.</param>
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

    /// <summary>
    /// Hides all page panels before displaying new content.
    /// </summary>
    public void ClearPages()
    {
        statsPanel.SetActive(false);
        cardPanel.SetActive(false);
        lorePanel.SetActive(false);
    }

    /// <summary>
    /// Displays either player stats or progression statistics,
    /// depending on the IsPlayerStats bool.
    /// </summary>
    /// <param name="stats">Player stat data.</param>
    /// <param name="weaponStats">Current weapon statistics.</param>
    private void ShowStats(PlayerStats stats, PlayerWeaponManager weaponStats)
    {
        if (stats == null) return;

        playerCollection = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>();
        gameData = FindAnyObjectByType<GameData>();

        string stageTrev = RunGameData.Instance.HasStarted ? $"{RunGameData.Instance.LevelCounter}" : "Unknown";
        string stageReq = RunGameData.Instance.HasStarted ? $"{RunGameData.Instance.LevelsBeforeBoss}" : "Unknown";

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
            $"Speed Bonus: {(stats.currentActionSpeedModifier - stats.baseActionSpeed) * 100}%\n";

            statsTextBottomLeft.text =
            $"-DAMAGE-\n" +
            $"Light Damage: {weaponStats.currentActiveWeaponData.LightDamage}\n" +
            $"Heavy Damage: {weaponStats.currentActiveWeaponData.HeavyDamage}\n" +
            $"Damage Bonus: {(stats.currentDamageModifier - 1) * 100}%\n";

            statsTextBottomRight.text =
            $"-CHANCE-\n" +
            $"Luck: {stats.currentLuck}%\n" +
            $"Critical Strike: {stats.currentCritChance}%\n";
        }
        else
        {
            statsTextTopLeft.text =
            $"-FAMILY-\n" +
            $"Family: {(playerCollection.playerContract != null ? playerCollection.playerContract.CardFamily.ToString() : "None")}\n";

            statsTextTopRight.text =
            $"-STAGE-\n" +
            $"Islands Traversed: {stageTrev} \n" +
            $"Islands Before Boss: {stageReq} \n";

            statsTextBottomLeft.text =
            $"-COMBAT- \n" +
            $"Player Deaths: {gameData.TotalPlayerDeath} \n" +
            $"Enemies Slain: {gameData.TotalEnemiesSlain} \n" +
            $"Bosses Slain: {gameData.TotalBossSlain} \n";

            statsTextBottomRight.text =
            $"-LOOT-\n" +
            $"Souls Collected: {gameData.TotalSoulsCollected}\n" +
            $"Souls Sacrificed: {gameData.TotalSoulsSacrificed}\n" +
            $"Cards Picked Up: {gameData.TotalCardsPickedup}\n";
        }
    }
    /// <summary>
    /// Populates the card page with card UI elements.
    /// Existing card objects are removed before new ones are created.
    /// </summary>
    /// <param name="cards">Cards to display on the page.</param>
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
          
        }
    }
}


