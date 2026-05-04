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
        CardSystem system = GameObject.Find("CardSystem").GetComponent<CardSystem>();

        if (card == null) return;
        if (system.CheckUnlocked(card) && !(permanentCards.Contains(card)))
        {
            permanentCards.Add(card);
        }
        else if(!temporaryCards.Contains(card)) 
        {
            temporaryCards.Add(card);
        }
        else
        {
            Event_System.instance?.OnDroopMultipleSouls.Invoke();
            // If the player already has that card. Invoke the delegate to Listerns(LootManager)
            //To tell the manager to drop multiple souls, to give the player something else.
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
