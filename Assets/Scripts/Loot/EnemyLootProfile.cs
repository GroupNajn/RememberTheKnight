using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemyLootProfile : MonoBehaviour
{
    /// <summary>
    /// Specifies the base rarity tier for the loot item.
    /// </summary>
    [Header("Base Loot")]
    public RarityTier baseRarity = RarityTier.Common;
    /// <summary>
    /// Specifies the set of card families that are allowed.
    /// </summary>
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
