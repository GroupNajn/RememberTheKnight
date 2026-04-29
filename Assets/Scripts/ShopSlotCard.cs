using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotCard : MonoBehaviour
{
    [SerializeField] private bool unlocked;
    [SerializeField] private Image cardImage;

    private ShopBoard board;

    void Awake()
    {
        cardImage = GetComponent<Image>();
    }
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

        //board.CardBuilder.InstantiateCardWithoutScripts(card);

        if (card != null && cardImage != null)
        {
            cardImage.sprite = card.cardImage;
        }
    }
}
