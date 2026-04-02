
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


// Script made by Henric some random date

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [Header("Loot_Table")]
    [SerializeField] List<Droppable> CommonLootTable;
    [SerializeField] List<Droppable> UncommonLootTable;
    [SerializeField] List<Droppable> rareLootTable;
    [SerializeField] List<Droppable> EpicLootTable;
    [SerializeField] List<Droppable> LegendaryLootTable;
    public HashSet<Droppable> DroppedLoot {get {return droppedLoot;}}
    HashSet<Droppable> droppedLoot;
    [SerializeField] Droppable soulPrefab;

    //[SerializeField] float oneItemDropChance;
    //[SerializeField] float twoItemDropChance;
    //[SerializeField] float threeItemDropChance;



    /*
     * Managers need to be initialized via Awake to get priority,
     * before all other GameObjects call and Subscribe to their Actions/Events, 
     */

    private void Awake()
    {
        instance = this;
        droppedLoot = new HashSet<Droppable>();
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
        Event_System.instance.OnEnemyKilled += GetOneRandomItemLoot;


    }

    private void OnDestroy()
    {
        if (Event_System.instance != null)
            Event_System.instance.OnEnemyKilled -= GetOneRandomItemLoot;
    }

    public void RegisterLoot(Droppable loot)
    {
        if (loot != null)
            droppedLoot.Add(loot);
    }

    public void UnregisterLoot(Droppable loot)
    {
        if (loot != null)
            droppedLoot.Remove(loot);
    }

    //public void CalculateLootTier(EnemyDamage enemy)
    //{
    //    float totalTierWight = 0;

    //}

    public void GetOneRandomItemLoot(EnemyDamage enemy)
    {
        float totalWeight = 0;

        foreach (var item in SwitchLootTable(enemy.tier))
            totalWeight += item.Weight;

        float roll = Random.Range(0, totalWeight);
        float current = 0;

        foreach (var item in SwitchLootTable(enemy.tier))
        {
            current += item.Weight;
            if (roll < current)
            {
                PrintPercentOnSelectedItem(current, totalWeight);
                Debug.Log(item);
                DropLoot(item, enemy);
                return;
            }
        }
    }

    private List<Droppable> SwitchLootTable(Tier tier)
    {
        switch (tier)
        {
            case Tier.Common:
                return CommonLootTable;
            case Tier.Uncommon:
                return UncommonLootTable;
            case Tier.Rare:
                return rareLootTable;
            case Tier.Epic:
                return EpicLootTable;
            case Tier.Legendary:
                return LegendaryLootTable;
            default:
                return CommonLootTable;

        }


    }



    public void DropLoot(Droppable item, EnemyDamage enemy)
    {
        Vector3 pos = enemy.transform.position;
        Instantiate(item, pos, Quaternion.identity);

    }


    private void PrintPercentOnSelectedItem(float itemW, float SumOfW)
    {
        float percent = (itemW / SumOfW) * 100;
        Debug.Log(percent);
    }




}
