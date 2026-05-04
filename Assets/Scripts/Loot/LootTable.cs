using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LootTable : MonoBehaviour
{
    public List<CardData> Common { get; private set; } = new List<CardData>();
    public List<CardData> Uncommon { get; private set; } = new List<CardData>();
    public List<CardData> Rare { get; private set; } = new List<CardData>();
    public List<CardData> Epic { get; private set; } = new List<CardData>();
    public List<CardData> Legendary { get; private set; } = new List<CardData>();


    void Start()
    {

    }

    void Update()
    {

    }


    // Enum LootTables (Wands || Cups) == Filter.  E.G( 10 items). 
    //public List<CardData> FilterLoot(LootTables lootFilter)
    //{




    //}
}
