using System.Collections.Generic;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{
    private GameObject lootManager;
    private CardSystem cardSystem;
    public CardBuilder CardBuilder { get; private set; }

    private List<ShopSlotCard> slots = new List<ShopSlotCard>();
    private List<CardData> randomizedCards = new List<CardData>();

    [field: SerializeField] public List<GameObject> RandomPosters { get; private set; } = new List<GameObject>();

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
        var allCards = cardSystem.GetAllCards();

        if (allCards == null || allCards.Count == 0)
            return;

        foreach (var slot in slots)
        {
            int randomIndex = Random.Range(0, allCards.Count);
            CardData randomCard = allCards[randomIndex];

            randomizedCards.Add(randomCard);
            slot.SetCard(randomCard);
        }
    }
}
