using UnityEngine;
using System.Collections.Generic;
public class PageData
{
    //made by Michaëla 2026-04-19
    public enum PageType { Stats, Cards, Lore }

    public PageType type;

    // Stats page data
    public PlayerStats stats;
    public PlayerWeaponManager weaponStats;

    // Cards page data
    public List<CardData> cards;

    // Lore page data
    public string loreTitle;
    public string loreText;
}

