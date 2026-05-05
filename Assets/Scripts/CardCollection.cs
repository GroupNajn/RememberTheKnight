using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;


public class CardCollection : MonoBehaviour
{
    [SerializeField] private HashSet<CardData> temporaryCards = new HashSet<CardData>();
    [SerializeField] private HashSet<CardData> equippedCards = new HashSet<CardData>();
    private CardContract cardContract;
    public CardContract PlayersCardContract
    {
        set => cardContract = value;
    }

    public HashSet<CardData> TemporaryCards
    {
        get => temporaryCards;
    }
    public HashSet<CardData> EquippedCards
    {
        get => equippedCards;
    }
    void Start()
    {

    }

    void Update()
    {

    }


    public void EquipCards(CardData card)
    {
        equippedCards.Clear();
        CardSystem system = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        if (card == null) return;

        if (!equippedCards.Contains(card) && equippedCards.Count < 4 && system.CheckUnlocked(card)) 
        {
            equippedCards.Add(card);
        }

    }


    // Logisk fel. Behöver fixas. !!!!!!
    public void AddToTempCollection(CardData card)
    {
        CardSystem system = GameObject.Find("CardSystem").GetComponent<CardSystem>();

        if (card == null) return;
        if (system.CheckUnlocked(card))
        {
            
        }
        else if(!temporaryCards.Contains(card)) 
        {
            temporaryCards.Add(card);
        }
        else
        {
            Event_System.instance?.OnDroopMultipleSouls?.Invoke();
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
        if (equippedCards.Contains(card.CardData))
        {
            equippedCards.Remove(card.CardData);
        }
    }

    public List<CardData> GetTempCardCollection()
    {
        return temporaryCards.ToList();
    }

    public List<CardData> GetEquippedCards()
    {
        return equippedCards.ToList();
    }

    public void ClearTemporaryCards()
    {
        temporaryCards.Clear();

    }


}
