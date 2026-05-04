using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBoardCardSlot : MonoBehaviour
{
    [field: SerializeField] public bool isLocked { get; private set; }

    [SerializeField] private ShopBoard board;
    [SerializeField] public CardData CurrentCard;

    public void SetCard(CardData card)
    {
        CurrentCard = card;

        if (isLocked)
        {
            int randomIndex = Random.Range(0, board.RandomPosters.Count);
            GameObject randomPoster = board.RandomPosters[randomIndex];

            Vector3 positionOffset = randomPoster.transform.localPosition;

            GameObject instance = Instantiate(randomPoster, this.transform.position, this.transform.rotation, this.transform);
            instance.transform.localPosition = randomPoster.transform.localPosition;

            return;
        }

        board.CardBuilder.InstantiateCardWithoutScripts(card, this.transform);
    }
}
