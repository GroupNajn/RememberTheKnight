
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;

// Script made by Henric some random date

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [Header("Loot_Table")]
    [SerializeField] List<Loot> soulTable;
    [SerializeField] List<Loot> CommonLootTable;
    [SerializeField] List<Loot> UncommonLootTable;
    [SerializeField] List<Loot> rareLootTable;
    [SerializeField] List<Loot> EpicLootTable;
    [SerializeField] List<Loot> LegendaryLootTable;

    [Header("New Loot Table")]

    [SerializeField] List<Soul> newSoulTable;
    [SerializeField] List<CardData> newCommonLootTable;
    [SerializeField] List<CardData> newUncommonLootTable;
    [SerializeField] List<CardData> newRareLootTable;
    [SerializeField] List<CardData> newEpicLootTable;
    [SerializeField] List<CardData> newLegendaryLootTable;





    public HashSet<Loot> DroppedLoot { get { return droppedLoot; } }
    HashSet<Loot> droppedLoot;
    [SerializeField] Loot soulPrefab;
    [SerializeField] List<float> amountChanceTable;

    [Header("CardSystem")]
    [SerializeField] private CardSystem cardSystem;
    [SerializeField] private CardBuilder builder;
    private PlayerStats playerStats;

    public CardSystem CardSystem
    {
        get => cardSystem;
    }
    [Header("Loot Drop Modifiers")]
    [SerializeField] private float multipleLootModifier = 1f;
    [SerializeField] private float luckChanceScaler = 1f; // It will multiply the current player % chance. If 2, double. If 3 tripple it etc...
    [SerializeField] private int maxLootAmount = 3;
    [SerializeField] private float soulDropModifier = 2.5f;

    [Header("Upgrae Tier Modifiers")]
    [SerializeField] private float CommonTierUpgrade = 1;
    [SerializeField] private float UncommonUpgradeTierModifier = 0.5f;
    [SerializeField] private float RareTierUpgradeModifier = 0.2f;
    [SerializeField] private float EpicTierUpgradeModifier = 0.18f;
    [SerializeField] private float LegendaryTierUpgradeModifier = 0.13f;


    /* TODO: Need to a way to add logic to instantiate a normal soul or a healing/charged soul.
     Logic to faouvrly drop more of the signed Contract-Family.


    Loot-Calculation => Enemy-Killed => Take Enemy Tier => Make A roll on CardFamily perferability =>
    Make a roll on which Item it will drop.
    Souls is garunteed. 

    Change Items Drop chance accordingly with the Luck Stat.  



    */
    /*
     * Managers need to be initialized via Awake to get priority,
     * before all other GameObjects call and Subscribe to their Actions/Events, 
     */

    /*                        |Luck Threshhold for Tier Upgrade. 
     * Tier 1-3  =>  Common   | Luck > 5 %
     * Tier 4-5  =>  Uncommon | Luck > 8 %
     * Tier 6-7  =>  Rare     | Luck > 11 %
     * Tier 8-9  =>  Epic     | Luck > 15 %
     * Tier 10+  => Legendary | Luck > 20 %
     *  
     * 
     */






    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Duplicate manager destroyed");
            Destroy(gameObject);
            return;
        }
        droppedLoot = new HashSet<Loot>();

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {


        if (Event_System.instance == null)
        {
            Debug.LogError("Event_System.instance is null in LootManager.Start()");
            return;
        }

        //Debug.Log("LootManager subscribed");
        //Event_System.instance.OnEnemyKilled += RollMultipuleLoot;
        Event_System.instance.OnEnemyKilledNew += TryToDropLoot;
        InitializeCardPools();
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
        {
            //Event_System.instance.OnEnemyKilled -= RollMultipuleLoot;
            Event_System.instance.OnEnemyKilledNew += TryToDropLoot;

        }
    }

    // Transforms the players luck float values to actual procentage. 
    // E.G 3 float will become 0.03, 3% 
    private float GetScaledLuckChance()
    {
        if (playerStats == null)
            playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        if (playerStats == null)
            return 0f;

        return playerStats.currentLuck * (luckChanceScaler / 100f);
        // Exempel : 5f * (1f / 100f) = 0.1f = 5% 
    }


    private int RollForMultipleLoot()
    {
        float chance = GetScaledLuckChance() * multipleLootModifier; // * 2 

        chance += 0.1f; // To make sure the player have atleast 25% to drop a loot. It will  be increase with higher luck. 
        int amount = 0;

        while (Random.value < chance) // Between [0.0 - 1.0] 
        {
            amount++;

            if (amount >= maxLootAmount)
                break;

            chance *= 0.15f; // will divide the chance by 2 for each successful increase drop amount.
        }

        return amount;
    }

    private List<CardData> FilterLoot(LootTables LootProfile)
    {
        var result = new List<CardData>();
        var cards = cardSystem.GetAllCards();

        foreach (CardData card in cards)
        {
            if (IsFamilyAllowed(card.cardFamily, LootProfile))
            {
                result.Add(card);
            }
        }

        return result;

    }

    private bool IsFamilyAllowed(CardFamily family, LootTables profile)
    {
        switch (family)
        {
            case CardFamily.Cups:
                return (profile & LootTables.Cups) != 0;

            case CardFamily.Swords:
                return (profile & LootTables.Swords) != 0;

            case CardFamily.Wands:
                return (profile & LootTables.Wands) != 0;

            case CardFamily.Pentacles:
                return (profile & LootTables.Pentacles) != 0;

            default:
                return false;
        }

    }


    // Boolean to check if A CardTier is within a RarityTier. 
    public bool IsTierInsideRarity(Tier tier, RarityTier rarity)
    {
        return rarity switch
        {
            RarityTier.Common =>
                tier >= Tier.I && tier <= Tier.III,

            RarityTier.Uncommon =>
                tier >= Tier.IV && tier <= Tier.V,

            RarityTier.Rare =>
                tier >= Tier.VI && tier <= Tier.VII,

            RarityTier.Epic =>
                tier >= Tier.VIII && tier <= Tier.IX,

            RarityTier.Legendary =>
            tier >= Tier.X,

            _ => false
        };
    }



   
    // Returns the Next RarityTier, used in the Upgrade Mechanic, to advance the loot into the next RarityTier Range. 
    private RarityTier GetNextRarity(RarityTier rarity)
    {
        switch (rarity)
        {
            case RarityTier.Common:
                return RarityTier.Uncommon;

            case RarityTier.Uncommon:
                return RarityTier.Rare;

            case RarityTier.Rare:
                return RarityTier.Epic;

            case RarityTier.Epic:
                return RarityTier.Legendary;

            default:
                return rarity;
        }
    }
    // Takes in RarityTier and calculates the chance of it upgrading based on 
    // Another scaling method, "GetUpgradeChance" it scales the chance depending on the input Tier to the method. 
    private RarityTier RollRarityUpgrade(RarityTier baseRarity)
    {
        RarityTier currentRarity = baseRarity;

        while (currentRarity != RarityTier.Legendary)
        {
            float upgradeChance = GetUpgradeChance(currentRarity);

            if (Random.value <= upgradeChance)
            {
                currentRarity = GetNextRarity(currentRarity);

                upgradeChance *= 0.65f; // Divide the chance by 2. 
            }
            else
            {
                break;
            }
        }

        return currentRarity;
    }



    private float GetUpgradeChance(RarityTier rarity)
    {
        // luckChance = 10 % at start. 
        float luckChance = GetScaledLuckChance();

        (float baseChance, float modifier) = rarity switch
        {                   //Cases
            RarityTier.Common => (0.11f, CommonTierUpgrade),                 // 0.11 + (0.10 * 1) 21% Chance | Common => UnCommon | Max Cap 111%
            RarityTier.Uncommon => (0.08f, UncommonUpgradeTierModifier),// 0.08 + (0.10 * 0.7 ) = 16% Chance | UnCommon => Rare Max Cap 78%
            RarityTier.Rare => (0.06f, RareTierUpgradeModifier),          // 0.06 + (0.10 * 0.3) = 9% Chance | Rare => Epic | Max Cap 36%
            RarityTier.Epic => (0.04f, EpicTierUpgradeModifier),          // 0.04 + (0.10 * 0.1) = 5% Chance | Epic => Legendary | Max Cap 14% 
            RarityTier.Legendary => (0f, 0f),
            _ => (0f, 0f) // Default Case
        };

        return Mathf.Clamp01(baseChance + (luckChance * modifier));
    }

    // Return a given range depending on the RarityTier enum, to decide on which loot Range it should choose from. 
    private (Tier min, Tier max) GetTierRange(RarityTier rarity)
    {
        return rarity switch
        {               //Cases
            RarityTier.Common => (Tier.I, Tier.III),
            RarityTier.Uncommon => (Tier.IV, Tier.V),
            RarityTier.Rare => (Tier.VI, Tier.VII),
            RarityTier.Epic => (Tier.VIII, Tier.IX),
            RarityTier.Legendary => (Tier.X, Tier.XIII),
            _ => (Tier.I, Tier.I) // Default case 
        };
    }

    public RarityTier GetRarityFromTier(Tier tier)
    {
        if (tier >= Tier.I && tier <= Tier.III)
            return RarityTier.Common;

        if (tier >= Tier.IV && tier <= Tier.V)
            return RarityTier.Uncommon;

        if (tier >= Tier.VI && tier <= Tier.VII)
            return RarityTier.Rare;

        if (tier >= Tier.VIII && tier <= Tier.IX)
            return RarityTier.Epic;

        return RarityTier.Legendary;
    }

    private List<CardData> FilterCardsByRarity(List<CardData> cards, RarityTier rarity)
    {
        List<CardData> result = new List<CardData>();

        var (min, max) = GetTierRange(rarity);

        foreach (CardData card in cards)
        {
            if (card.cardTier >= min && card.cardTier <= max)
            {
                result.Add(card);
            }
        }

        return result;
    }

    private CardData GetRandomCard(List<CardData> cards)
    {
        int randomIndex = Random.Range(0, cards.Count);
        return cards[randomIndex];
    }

    public void TryToDropLoot(EnemyLootProfile profile, Vector3 spawnPos)
    {
        if (profile == null) return;

        SpawnSoul(spawnPos);

        float soulChance = 1f + GetScaledLuckChance();
        if (RollForChargedSoul(soulChance))
        {
            SpawnChargedSoul(spawnPos);
        }

        int lootAmount = RollForMultipleLoot();

        List<CardData> allowedCards = FilterLoot(profile.allowedFamiles);

        if(allowedCards.Count <= 0)
        {
            Debug.LogWarning("No allowed cards found in LootProfile");
            return;
        }

        for(int i = 0; i < lootAmount; i++)
        {
            RarityTier finalRarity = RollRarityUpgrade(profile.baseRarity);

            List<CardData> cardsInRarity = FilterCardsByRarity(allowedCards, finalRarity);

            if(cardsInRarity.Count <= 0)
            {
                Debug.LogWarning("No cards found in rarity" + finalRarity);
            }
            CardData selectedCard = GetRandomCard(cardsInRarity);

            SpawnCard(selectedCard, spawnPos);
            // Add a new method inside of CardBuilder to Instantiate a new Card. After that it is finished. 
        }

    }

    private bool RollForChargedSoul(float chance)
    {
        float baseChance = 1f;

        float overflow = chance - baseChance;

        if (overflow <= 0f)
            return false;

        return Random.value < overflow;
    }

    // Methods below handle the loot dropping and which item to drop depending on loot table. 



    public void RegisterLoot(Loot loot)
    {
        if (loot != null)
            droppedLoot.Add(loot);
    }

    public void UnregisterLoot(Loot loot)
    {
        if (loot != null)
            droppedLoot.Remove(loot);
    }

    public void GetOneRandomItemLoot(EnemyDamage enemy)
    {
        float totalWeight = 0;

        foreach (var item in SwitchLootTable(enemy.tier)) // Add all weights from items for the respective loot table
            totalWeight += item.Weight;

        float roll = Random.Range(0, totalWeight); // Make a roll from 0 to the sum of all weights. (e.g) 0-250
        float current = 0;

        foreach (var item in SwitchLootTable(enemy.tier))
        {
            current += item.Weight;
            if (roll < current)
            {
                PrintPercentOnSelectedItem(current, totalWeight);
                DropLoot(item, enemy);

                return;
            }
        }
    }

    private List<Loot> SwitchLootTable(RarityTier tier)
    {
        switch (tier)
        {
            case RarityTier.Common:
                return CommonLootTable;
            case RarityTier.Uncommon:
                return UncommonLootTable;
            case RarityTier.Rare:
                return rareLootTable;
            case RarityTier.Epic:
                return EpicLootTable;
            case RarityTier.Legendary:
                return LegendaryLootTable;
            default:
                return CommonLootTable;

        }


    }
    public void RollMultipuleLoot(EnemyDamage enemy)
    {
        for (int i = 0; i < CalculateLootAmount(); i++)
        {
            GetOneRandomItemLoot(enemy);
        }
    }

    private int CalculateLootAmount()
    {
        float totalWeight = 0;
        foreach (var weight in amountChanceTable)
        {
            totalWeight += weight;
        }

        float roll = Random.Range(0, totalWeight);
        float current = 0;

        for (int i = 0; i < amountChanceTable.Count; i++)
        {
            current += amountChanceTable[i];

            if (roll < current)
            {
                return i + 1;
            }
        }

        return 1;
    }

    public void DropLoot(Loot item, EnemyDamage enemy)
    {
        Vector3 pos = enemy.transform.position;
        Instantiate(item, pos + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    public void SpawnSoul(Vector3 spawnPos)
    {
        Instantiate(newSoulTable[0], spawnPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
    public void SpawnChargedSoul(Vector3 spawnPos)
    {
        Instantiate(newSoulTable[1], spawnPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }

    public void SpawnCard(CardData card, Vector3 spawnPos)
    {
        builder.InstatitateCard(card, spawnPos);
    }


    private void PrintPercentOnSelectedItem(float itemW, float SumOfW)
    {
        float percent = (itemW / SumOfW) * 100;
        Debug.Log("%" + percent);
    }

    public void ReDesributeWeight(List<Loot> table)
    {
        float totalWeight = 0;
        int itemsInTable = 0;
        foreach (var item in table)
        {
            itemsInTable++;
            totalWeight += item.Weight;
        }
        if (totalWeight < 100)
        {
            float missingWeight = (totalWeight - 100);
            float allocatWeight = Mathf.Abs(missingWeight) / itemsInTable;

        }


    }

    private void InitializeCardPools()
    {
        var cards = cardSystem.GetAllCards();

        foreach (var card in cards)
        {
            RarityTier rarity = GetRarityFromTier(card.cardTier);

            switch (rarity)
            {
                case RarityTier.Common:
                    newCommonLootTable.Add(card);
                    break;

                case RarityTier.Uncommon:
                    newUncommonLootTable.Add(card);
                    break;

                case RarityTier.Rare:
                    newRareLootTable.Add(card);
                    break;

                case RarityTier.Epic:
                    newEpicLootTable.Add(card);
                    break;

                case RarityTier.Legendary:
                    newLegendaryLootTable.Add(card);
                    break;
            }
        }
    }






}
