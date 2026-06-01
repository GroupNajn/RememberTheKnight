using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{

    /// <summary>
    /// Created by Anton 2026-04-28
    /// Initially created as a component to handle the shop board, which is the object that the player interacts with to open the card shop UI.
    /// Also uses the ShopBoardCardSlot component to populate the shop board slots with cards that the player can purchase.
    /// </summary>
    private GameObject lootManager;
    private CardSystem cardSystem;
    public CardBuilder CardBuilder { get; private set; }

    private List<ShopBoardCardSlot> slots = new List<ShopBoardCardSlot>();
    public List<ShopBoardCardSlot> Slots => slots;

    [field: SerializeField] public List<GameObject> RandomPosters { get; private set; } = new List<GameObject>();


    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ShopBoardCardSlot>(true));
    }

    void Start()
    {
        lootManager = GameObject.FindWithTag("LootManager");
        cardSystem = lootManager.GetComponentInChildren<CardSystem>(true);
        CardBuilder = lootManager.GetComponentInChildren<CardBuilder>(true);

        PopulateSlots();
    }

    void PopulateSlots()
    {
        var allCards = LootManager.instance.RollSevenRewardCards();
        //var allCards = cardSystem.GetAllCards();

        if (allCards == null || allCards.Count == 0)
            return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= allCards.Count)
            {
                slots[i].SetLocked(true);
                slots[i].SetCard(null);
            }
            else
            {
                CardData randomCard = allCards[i];
                slots[i].SetCard(randomCard);
            }
        }
    }
}