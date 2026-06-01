using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CardCollection : MonoBehaviour
{
    [SerializeField] private HashSet<CardData> temporaryCards = new HashSet<CardData>();
    [SerializeField] private HashSet<CardData> equippedCards = new HashSet<CardData>();
    [SerializeField] private HashSet<CardData> equippedCourtCards = new HashSet<CardData>();
    private const int cardLimit = 5;
    private CardContract cardContract;
    public CardContract PlayersCardContract
    {
        set => cardContract = value;
    }

    public HashSet<CardData> EquippedCourtCards
    {
        get => equippedCards;
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
        equippedCourtCards.Clear();
    }

    public void EquipCards(CardData card)
    {
        CardSystem system = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        if (card == null) return;

        if (!equippedCards.Contains(card) && equippedCards.Count < cardLimit && system.CheckUnlocked(card) && !temporaryCards.Contains(card))
        {
            equippedCards.Add(card);
        }
    }

    public void EquipCourtCard(CardData card)
    {
        equippedCourtCards.Add(card);
    }



    public void AddToTempCollection(CardData card)
    {
        if (card == null) return;

        bool existsInTemp = temporaryCards.Any(c => c != null && c.cardID == card.cardID);
        bool existsInEquipped = equippedCards.Any(c => c != null && c.cardID == card.cardID);

        if (existsInTemp)
        {

            Event_System.instance?.OnDroopMultipleSouls?.Invoke(card);
            return;
        }

        if (existsInEquipped)
        {

            Event_System.instance?.OnDroopMultipleSouls?.Invoke(card);
            return;
        }

        temporaryCards.Add(card);

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
    public List<CardData> GetEquippedCourtCards()
    {
        return equippedCourtCards.ToList();
    }
    public List<CardData> ReturnCardsForApplyingStats() // Sorts and removes the duplicates in the concatinated list to return. Sorts by 
    {
        var tempPlusEquipped  = equippedCards.Concat(temporaryCards).GroupBy(card => card.cardID).Select(group => group.First()).ToList();
        return tempPlusEquipped.Concat(equippedCourtCards).GroupBy(card => card.cardID).Select(group => group.First()).ToList();
    }

    public void ClearTemporaryCards()
    {
        temporaryCards.Clear();

    }
    public void ClearCourtCards()
    {
        equippedCourtCards.Clear();
    }

    public void ClearEquipedCards()
    {
        equippedCards.Clear();
    }


}
