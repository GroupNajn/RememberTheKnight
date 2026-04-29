using System.Collections.Generic;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{
    private GameObject lootManager;
    private CardSystem cardSystem;
    public CardBuilder CardBuilder { get; private set; }

    private List<ShopSlotCard> slots = new List<ShopSlotCard>();
    private List<CardData> randomizedCards = new List<CardData>();

    [SerializeField] public List<GameObject> randomPosters { get; private set; } = new();

    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ShopSlotCard>(true));
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
        var unlockedCards = cardSystem.GetUnlockedCards();

        if (unlockedCards == null || unlockedCards.Count == 0)
            return;

        foreach (var slot in slots)
        {
            int randomIndex = Random.Range(0, unlockedCards.Count);
            CardData randomCard = unlockedCards[randomIndex];

            randomizedCards.Add(randomCard);
            slot.SetCard(randomCard);
        }
    }
}
