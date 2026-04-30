using UnityEngine;

public enum RarityTier { Common = 1, Uncommon = 2, Rare = 3, Epic = 4, Legendary  = 5}
public enum PickableState { NotPickable, Pickable }

public enum Tier
{
    I = 1,
    II = 2,
    III = 3,
    IV = 4,
    V = 5,
    VI = 6,
    VII = 7,
    VIII = 8,
    IX = 9,
    X = 10,
    XI = 11,
    XII = 12,
    XIII = 13,
}

public enum LootTables { 
    None = 0,
    Swords = 1 << 1,
    Cups = 1 << 2,
    Wands = 1 << 3,
    Pentacles = 1 << 4,
    Page = 1 << 5,
    Knight = 1 << 6,
    Queen = 1 << 7,
    King = 1 << 8,
    CourtOnly = 1 << 9

}



public enum CardFamily
{
    None = 0,
    Cups = 1,
    Swords = 2,
    Pentacles = 3,
    Wands = 4
}

public enum CardUnlockType
{
    Permanent,
    Temporary
}
