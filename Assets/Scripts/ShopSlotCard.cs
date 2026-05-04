using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotCard : MonoBehaviour
{
    [SerializeField] private bool unlocked;
    [SerializeField] private Image cardImage;

    [SerializeField] private ShopBoard board;

    void Awake()
    {
        cardImage = GetComponent<Image>();
    }
    void Start()
    {
        
    }
    public void SetCard(CardData card)
    {
        if (!unlocked)
        {
            int randomIndex = Random.Range(0, board.RandomPosters.Count);
            GameObject randomPoster = board.RandomPosters[randomIndex];
            
            Vector3 positionOffset = randomPoster.transform.localPosition;

            GameObject instance = Instantiate(randomPoster, this.transform.position, this.transform.rotation, this.transform);
            instance.transform.localPosition = randomPoster.transform.localPosition;

            return;
        }

        board.CardBuilder.InstantiateCardWithoutScripts(card, this.transform);

        if (card != null && cardImage != null)
        {
            cardImage.sprite = card.cardImage;
        }
    }
}
