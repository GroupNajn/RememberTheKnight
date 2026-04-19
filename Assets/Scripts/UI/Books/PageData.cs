using UnityEngine;
using System.Collections.Generic;
public class PageData
{
    public enum PageType { Stats, Cards, Text }

    public PageType type;

    public PlayerStats stats;
    public List<CardData> cards;
    public string text;
}

