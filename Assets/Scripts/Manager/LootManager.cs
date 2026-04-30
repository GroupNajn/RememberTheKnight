
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UnityEditor.UIElements;


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
    public HashSet<Loot> DroppedLoot {get {return droppedLoot;}}
    HashSet<Loot> droppedLoot;
    [SerializeField] Loot soulPrefab;
    [SerializeField] List<float> amountChanceTable;

    [Header("CardSystem")]
    [SerializeField] private CardSystem cardSystem;

    private PlayerStats playerStats;

    public CardSystem CardSystem
    {
        get => cardSystem;
    }
    [Header("Loot Drop Modifiers")]
    [SerializeField] private float multipleLootModifier = 2f;
    [SerializeField] private float tierUpgradeModifier = 2f; 
    [SerializeField] private float luckChanceScaler = 1f; // It will multiply the current player % chance. If 2, double. If 3 tripple it etc...
    [SerializeField] private int maxLootAmount = 3;


   
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
        Event_System.instance.OnEnemyKilled += RollMultipuleLoot;
    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
            Event_System.instance.OnEnemyKilled -= RollMultipuleLoot;
    }

    // Transforms whole float values to actual procentage. 
    // E.G 3 float will become 0.03, 3% 
    private float GetScaledLuckChance() 
    {
        if (playerStats == null)
            playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        if (playerStats == null)
            return 0f;

        return playerStats.currentLuck * (luckChanceScaler / 100f);
        // Exempel : 5f * (2f / 100f) = 0.1f = 10% 
    }

    private bool RollForTierUpgrade()
    {
        float chance = GetScaledLuckChance();

        // Roll between 1.0 - 0.0.
        // 

        return Random.value < chance;
    }

    private Tier RollTierUpgrade(Tier currentTier)
    {
        if (!RollForTierUpgrade())
            return currentTier;

        int nextTier = (int)currentTier + 1;
        int maxTier = System.Enum.GetValues(typeof(Tier)).Length;

        nextTier = Mathf.Clamp(nextTier, 1, maxTier);

        return (Tier)nextTier;
    }

    private int RollForMultipleLoot()
    {
        float chance = GetScaledLuckChance() * multipleLootModifier;

        int amount = 1;

        while (Random.value < chance) // Between [0.0 - 1.0] 
        {
            amount++;

            if (amount >= maxLootAmount)
                break;

            chance *= 0.5f; // will divide the chance by 2 for each successful increase drop amount.
        }

        return amount;
    }

    public void TryToDropLoot()
    {
        float chance = GetScaledLuckChance();



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
        Instantiate(item, pos + new Vector3(0,0.5f,0), Quaternion.identity);

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
        if(totalWeight < 100)
        {
            float missingWeight = (totalWeight - 100);
            float allocatWeight = Mathf.Abs(missingWeight) / itemsInTable;

        }


    }




}
