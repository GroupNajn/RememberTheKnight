
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
[CreateAssetMenu(menuName = "Managers/Loot Manager")]

public class LootManager : ScriptableObject
{
    [SerializeField] Event_System EventSystem;
    // Script by Henric 2026-03-15
    [Header("Loot_Table")]
    [SerializeField] List<Droppable> lootTable;
    private HashSet<Droppable> droppedLoot;
    [SerializeField] Droppable soulPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //private HashSet<IDroppable> lootTable = new HashSet<IDroppable>();
    private float oneItemDropChance;
    private float twoItemDropChance;
    private float threeItemDropChance;
    /* These Variables are going to be used in similar method to calculate the odds of 
       Multipule items being dropped. 
    */
    //void Start()
    //{
    //    oneItemDropChance = 80.0f;
    //    twoItemDropChance = 15.0f;
    //    threeItemDropChance = 5.0f;


    //    //PrintPercentOnSelectedItem();
    //}

    public void RegisterLoot(Droppable loot)
    {
        droppedLoot.Add(loot);
    }

    public void UnregisterLoot(Droppable loot)
    {
        droppedLoot.Remove(loot);
    }
    private void OnEnable()
    {
        EventSystem.OnEnemyKilled += GetOneRandomItemLoot;
    }
    private void OnDisable()
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
    public void GetOneRandomItemLoot(EnemyDamage enemy) // The higher the Weight on the items the higher probability of being chosen
    {

        float totalWeight = 0;

        foreach (Droppable item in lootTable)
            totalWeight += item.Weight;

        float roll = Random.Range(0, totalWeight);
        float current = 0;

        foreach (Droppable item in lootTable)
        {
            current += item.Weight;
            if (roll < current) 
            {

                PrintPercentOnSelectedItem(current, totalWeight);

              DropLoot(item, enemy);
            }
        }
        return;
    }

    public void DropLoot(Droppable item, EnemyDamage enemy)
    {
        Vector3 pos = enemy.Position;
        item.position = pos;
        Droppable droppedItem = Instantiate(item, pos, Quaternion.identity);
       
    }

    private void PrintPercentOnSelectedItem(float itemW, float SumOfW)
    {

        float percent = (itemW / SumOfW) * 100;
        Debug.Log(percent);
    }
}
