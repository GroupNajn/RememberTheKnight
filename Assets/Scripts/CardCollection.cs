using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    private List<Card> temporaryCards = new List<Card>();
    private HashSet<Card> permanentCards = new HashSet<Card>();
    // HashSet to only one copy of a permanent cards.
    // No duplicates. 
    void Start()
    {

    }

    void Update()
    {

    }

    public void AddToCollection(Card card)
    {
        CardData cardData = card.CardData;
        if (card.CardData.unlockType == CardUnlockType.Permanent && !(permanentCards.Contains(card)))
        {
            permanentCards.Add(card);
        }
        else
        {
            temporaryCards.Add(card);
        }
    }

    public void RemoveFromTemporaryCollection(Card card)
    {
        temporaryCards.Remove(card);
    }

    public void RemoveFromPermanentCollection(Card card)
    {
        if (permanentCards.Contains(card))
        {
            permanentCards.Remove(card);
        }
    }

    public void ResetTemporaryCards()
    {
        temporaryCards.Clear();

    }


}
