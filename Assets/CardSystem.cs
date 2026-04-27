using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards = new();
    [SerializeField] private List<CardData> unlockedCards = new();
    

    private int unlockedTier = (int)Tier.I;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public IReadOnlyList<CardData> GetUnlockedCards()
    {
        return unlockedCards;
    }


    public void UnlockCardsAfterSigningContract(CardContract contract)
    {
        foreach(CardData cardData in allCards)
        {
            if (cardData.cardFamily == contract.CardFamily && unlockedTier == (int)cardData.cardTier)
                unlockedCards.Add(cardData);
        }


    }

    //public List<CardData> GetDroppableCards()
    //{
    //    List<CardData> droppableCards = new List<CardData>();

    //    foreach (CardData card in unlockedCards)
    //    {

    //    }

    //}




}
