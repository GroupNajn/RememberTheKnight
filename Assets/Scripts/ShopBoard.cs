using System.Collections.Generic;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{
    private LootManager lootManager;
    [SerializeField] private CardSystem cardSystem;

    private List<ShopSlotCard> slots = new List<ShopSlotCard>();

    [SerializeField] public List<GameObject> randomPosters { get; private set; } = new();

    void Awake()
    {
        lootManager = GetComponent<LootManager>();
        slots.AddRange(GetComponentsInChildren<ShopSlotCard>(true));
    }

    void Start()
    {
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
