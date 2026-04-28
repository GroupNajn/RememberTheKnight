using System.Collections.Generic;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{
    private Loot_System lootSystem;
    private CardSystem cardSystem;

    private List<ShopSlotCard> slots = new List<ShopSlotCard>();

    [SerializeField] public List<GameObject> randomPosters { get; private set; } = new();

    void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ShopSlotCard>());
    }

    void Start()
    {
        cardSystem = lootSystem.GetComponentInChildren<CardSystem>();
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

            slot.SetCard(randomCard);
        }
    }
}
