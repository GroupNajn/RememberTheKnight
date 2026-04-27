using System.Collections.Generic;
using System.Security;
using UnityEngine;


public class CardCollection : MonoBehaviour
{
    [SerializeField] private List<Card> temporaryCards = new List<Card>();
    [SerializeField] private List<Card> permanentCards = new List<Card>();

    public List<Card> TemporaryCards
    {
        get => temporaryCards;
    }
    public List<Card> PermanentPermanentCards
    {
        get => permanentCards;
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void AddToCollection(Card card)
    {
        if (card == null) return;
        if (card.CardUnlockType == CardUnlockType.Permanent && !(permanentCards.Contains(card)))
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

    public List<Card> GetTempCardCollection()
    {
        return temporaryCards;
    }

    public List<Card> GetPermanentCardCollection()
    {
        return permanentCards;
    }

    public void ResetTemporaryCards()
    {
        temporaryCards.Clear();

    }


}
