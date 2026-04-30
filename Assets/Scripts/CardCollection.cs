using System.Collections.Generic;
using System.Security;
using UnityEngine;


public class CardCollection : MonoBehaviour
{
    [SerializeField] private List<CardData> temporaryCards = new List<CardData>();
    [SerializeField] private List<CardData> permanentCards = new List<CardData>();
    private CardContract cardContract;
    public CardContract PlayersCardContract
    {
        set => cardContract = value;
    }

    public List<CardData> TemporaryCards
    {
        get => temporaryCards;
    }
    public List<CardData> PermanentPermanentCards
    {
        get => permanentCards;
    }
    void Start()
    {

    }

    void Update()
    {

    }

    public void AddToCollection(CardData card)
    {
        if (card == null) return;
        if (card)
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
        temporaryCards.Remove(card.CardData);
    }

    public void RemoveFromPermanentCollection(Card card)
    {
        if (permanentCards.Contains(card.CardData))
        {
            permanentCards.Remove(card.CardData);
        }
    }

    public List<CardData> GetTempCardCollection()
    {
        return temporaryCards;
    }

    public List<CardData> GetPermanentCardCollection()
    {
        return permanentCards;
    }

    public void ResetTemporaryCards()
    {
        temporaryCards.Clear();

    }


}
