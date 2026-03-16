
using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{

    // Script by Henric 2026-03-15

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static LootManager instance;
    public List<Loot_Item> lootTable = new List<Loot_Item>();
    private float oneItemDropChance;
    private float twoItemDropChance;
    private float threeItemDropChance;
    /* These Variables are going to be used in similar method to calculate the odds of 
       Multipule items being dropped. 
    */
    void Start()
    {
        oneItemDropChance = 80.0f;
        twoItemDropChance = 15.0f;
        threeItemDropChance = 5.0f;
        //PrintPercentOnSelectedItem();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    SUMMARY: IF there are 3 Items with the following weight: Item:1 = 5, Item:2 = 10, Item:3 = 20.
            The sum of those weights will be 35.
            Then if the roll is 18. Item:3 will be chosen. 
            Because the two previous Items will have a combined weight of 15. Since we know that Item:3 has a weight of 20. 
            Only 20 units will be left on the Number Axis, and once we add Item:3's weight to the "current" variable, roll will be < current
            in the second for each loop. The output will be Item:3
    */
    public Loot_Item GetOneRandomItemLoot() // The higher the Weight on the items the higher probability of being chosen
    {
        float totalWeight = 0;

        foreach (Loot_Item item in lootTable)
            totalWeight += item.weight;

        float roll = Random.Range(0, totalWeight);
        float current = 0;

        foreach (Loot_Item item in lootTable)
        {
            current += item.weight;
            if (roll < current)
                PrintPercentOnSelectedItem(current, totalWeight);
            return item;
        }
        return null;
    }

    //public LootItem GetDifferentAmountOfLoot()
    //{



    //}

    private void PrintPercentOnSelectedItem(float itemW, float SumOfW)
    {

        float percent = (itemW / SumOfW) * 100;
        Debug.Log(percent);
    }
}
