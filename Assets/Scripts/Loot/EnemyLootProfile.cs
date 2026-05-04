using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemyLootProfile : MonoBehaviour
{

    [Header("Base Loot")]
    public RarityTier baseRarity = RarityTier.Common;

    [Header("Allowed Card Families")]
    public LootTables allowedFamiles =
        LootTables.Cups |
        LootTables.Swords |
        LootTables.Wands |
        LootTables.Pentacles;

    [Header("Drops")]
    public bool canDropCards = true;
    public bool canDropSouls = true;

    void Start()
    {

    }

    void Update()
    {

    }


}
