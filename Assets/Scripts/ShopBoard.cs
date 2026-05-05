using System.Collections.Generic;
using UnityEngine;

public class ShopBoard : MonoBehaviour
{
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
        var allCards = cardSystem.GetAllCards();

        if (allCards == null || allCards.Count == 0)
            return;

        foreach (var slot in slots)
        {

            if (slot.isLocked)
            {
                slot.SetCard(null);
            }
            else
            {
                int randomIndex = Random.Range(0, allCards.Count);
                CardData randomCard = allCards[randomIndex];
                slot.SetCard(randomCard);
            }
        }
    }
}