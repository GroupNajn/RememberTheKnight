using UnityEngine;
using System.Collections.Generic;
public class PageData
{
    //made by Michaëla 2026-04-19
    public enum PageType { Stats, Cards, Lore }

    public PageType type;

    public PlayerStats stats;
    public PlayerWeaponManager weaponStats;
    public List<CardData> cards;

    public string loreTitle;
    public string loreText;
}

