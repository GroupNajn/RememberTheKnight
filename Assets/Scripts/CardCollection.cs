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
        temporaryCards.Clear();
        equippedCards.Clear();

        
    }
    private void OnDisable()
    {
        
    }

    void Update()
    {

    }


    public void EquipCards(CardData card)
    {
        CardSystem system = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        if (card == null) return;

        if (!equippedCards.Contains(card) && equippedCards.Count < 4 && system.CheckUnlocked(card) && !temporaryCards.Contains(card)) 
        {
            equippedCards.Add(card);
        }
        Debug.Log($"ANTAL KORT I EQUIPPED {equippedCards.Count}"); 

    }


    // Logisk fel. Behöver fixas. !!!!!!
    public void AddToTempCollection(CardData card)
    {
        if (card == null) return;
        if(temporaryCards.Contains(card))
        {
            Debug.Log($"{card.cardID} Finns Redan I TemporaryCards");
        }
        if (!temporaryCards.Contains(card) && !equippedCards.Contains(card))
        {
            temporaryCards.Add(card);
            NotifyCardPickUp();
        }
        else
        {
            Event_System.instance?.OnDroopMultipleSouls?.Invoke();
            // If the player already has that card. Invoke the delegate to Listerns(LootManager)
            //To tell the manager to drop multiple souls, to give the player something else.
        }
        Debug.Log($"ANTAL KORT Temporary {TemporaryCards.Count}");
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
    public List<CardData> ReturnCardsForApplyingStats() // Sorts and removes the duplicates in the concatinated list to return. Sorts by 
    {
        return equippedCards.Concat(temporaryCards).GroupBy(card => card.cardID).Select(group => group.First()).ToList();
    }

    public void ClearTemporaryCards()
    {
        temporaryCards.Clear();

    }

    public void ClearEquipedCards()
    {
        equippedCards.Clear();
    }

    private void NotifyCardPickUp()
    {
        Event_System.instance?.OnCardPickedUp?.Invoke();
    }


}
