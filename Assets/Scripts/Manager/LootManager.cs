
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


// Script made by Henric some random date

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [Header("Loot_Table")]
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

    public CardSystem CardSystem
    {
        get => cardSystem;
    }


   
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
        //oneItemDropChance = 80.0f;
        //twoItemDropChance = 15.0f;
        //threeItemDropChance = 5.0f;

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
