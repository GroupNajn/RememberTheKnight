using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;


public class CardCollection : MonoBehaviour
{
    [SerializeField] private HashSet<CardData> temporaryCards = new HashSet<CardData>();
    [SerializeField] private HashSet<CardData> permanentCards = new HashSet<CardData>();
    private CardContract cardContract;
    public CardContract PlayersCardContract
    {
        set => cardContract = value;
    }

    public HashSet<CardData> TemporaryCards
    {
        get => temporaryCards;
    }
    public HashSet<CardData> PermanentPermanentCards
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
        return temporaryCards.ToList();
    }

    public List<CardData> GetPermanentCardCollection()
    {
        return permanentCards.ToList();
    }

    public void ResetTemporaryCards()
    {
        temporaryCards.Clear();

    }


}
