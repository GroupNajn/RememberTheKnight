using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards = new();
    [SerializeField] private List<CardData> unlockedCards = new();
    private CardContract cardContract;

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

    //Method to be called after a contract is signed and is no longer null.
    // To set the Unlocked Cards at start. 
    public void UnlockCardsAfterSigningContract(CardContract contract)
    {
        foreach(CardData cardData in allCards)
        {
            if (cardData.cardFamily == contract.CardFamily && unlockedTier == (int)cardData.cardTier)
                unlockedCards.Add(cardData);
        }
    }

    // Need the reference on the presumed created and signed contract.
    public void UnlockDroppedCardInSignedFamily(CardData card)
    {

        if((int)card.cardTier == unlockedTier + 1 && card.cardFamily == cardContract.CardFamily)
        {

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
