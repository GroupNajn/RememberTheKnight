using System.Collections.Generic;
using UnityEngine;

public class ShopSlotCard : MonoBehaviour
{
    [SerializeField] private bool unlocked;

    private ShopBoard board;

    void Start()
    {
        board = GetComponentInParent<ShopBoard>();
    }
    public void SetCard(CardData card)
    {
        if (!unlocked)
        {
            int randomIndex = Random.Range(0, board.randomPosters.Count);
            GameObject randomPoster = board.randomPosters[randomIndex];

            Instantiate(randomPoster, this.transform);
        }

        Debug.Log($"Slot {name} card {card.name}");

        Instantiate(card, this.transform);
    }
}
