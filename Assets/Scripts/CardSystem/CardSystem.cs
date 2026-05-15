using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    // Only to show in inspectorn and to store all the cards at start. 
    [SerializeField] private List<CardData> allCards = new();
    [SerializeField] private List<CardData> unlockedCards = new();
    // Hashsets to not have Duplicates.  
    private HashSet<CardData> hashUnlocked = new HashSet<CardData>();
  

    // Script made by Henric in the end of april 2026. 

    public CardContract PlayerConract
    {
        set => cardContract = value;
    }
    private CardContract cardContract;

    public int UnlockedTier { get => unlockedTier; }


    private int unlockedTier = (int)Tier.I;

   
    void Start()
    {

        InitializeLockCards();
        //UnlockAllTierOneToThreeTemporary();
       

    }
    //Temporary Method to return a randomCardData in the allcards list. 
    public CardData ReturnRandomCard()
    {

        int index = Random.Range(0, allCards.Count);
        return allCards[index];
    }

    //Method to be called after a contract is signed and is no longer null.
    // To set the Unlocked Cards at start. 
    // INFO AFTER VERTICAL SLICE 2. METHOD SHOULD NO LONGER UNLOCK UP TO A TIER.
    
    public void InitializeLockCards()
    {
        foreach (CardData cardData in allCards)
        {
            if (cardData.cardTier == Tier.I)
            {
                    unlockedCards.Add(cardData);
                    hashUnlocked.Add(cardData);
            }
        }
    }

    public List<CardData> ReturnAllCardsOneTierAbove()
    {
        List<CardData> tempList = new List<CardData>();

        foreach (CardData card in allCards)
        {
            if ((int)card.cardTier == unlockedTier + 1)
            {
                tempList.Add(card);
            }
        }
        return tempList;
    }


    // Need the reference on the presumed created and signed contract Object.
    // Adds a new cardData to unlocked cards list.
    // And increases the unlocked-Tier condition variable. 
    public void UnlockDroppedCardInSignedFamily(CardData card)
    {
        if ((int)card.cardTier == unlockedTier + 1 && card.cardFamily == cardContract.CardFamily)
        {
            unlockedCards.Add(card);
            unlockedTier++;
            Mathf.Clamp(unlockedTier, (int)Tier.I, (int)Tier.XIII);
        }

    }

    public void UnlockCardFromDonation(CardData card)
    {
        unlockedCards.Add(card);
        hashUnlocked.Add(card);
    }

    //Method is to be used in unison when a card is picked up, to check if the 
    //The condition to increase the unlockableTier, it checks if the card level is 1 above
    // the current unlockableTier, and also if the card is the same family as the cardContract. 
    public bool CheckIncreaseUnlockTier(CardData card)
    {
        if ((int)card.cardTier == unlockedTier + 1 && card.cardFamily == cardContract.CardFamily)
        {
            unlockedTier++;
            return true;
        }
        else return false;
    }


    //Basic Method to increment unlockableTier with an int amount.
    //Clamps it between the Min and Max Tiers.
    public void IncreaseUnlockTier(int levelIncrease)
    {
        unlockedTier += levelIncrease;
        Mathf.Clamp(unlockedTier, (int)Tier.I, (int)Tier.XIII);
    }



    // Checks if a card is unlocked. 
    public bool CheckUnlocked(CardData card)
    {
        return hashUnlocked.Contains(card);
    }


    /*<summary> Method is a test method used for the GameHabitat game show.
     *  It is to be removed later when the proper implementation of the the card signing contract is finished
     * and this test method is no longer valid. 
     * 
     * 
     * 
     */
    public void UnlockAllTierOneToThreeTemporary()
    {
        foreach (CardData card in allCards)
        {
            if ((int)card.cardTier > 4) continue;

            unlockedCards.Add(card);

        }
    }

    //Initializes the The HashSet that is to be used outside of the Class itself.
    // To avoide duplicates in other algorithms, to prevent unwanted behavior. 

    public IReadOnlyList<CardData> GetUnlockedCards()
    {
        return unlockedCards;
    }

    public IReadOnlyList<CardData> GetAllCards()
    {
        return allCards;
    }

    public CardData GetNextCardInSelectedFamily()
    {
        List<CardData> sortedCards = new List<CardData>(allCards);

        foreach (CardData data in sortedCards.ToArray())
        {
            Debug.Log($"Looping through sorted cards");
            if (data.cardFamily != cardContract.CardFamily || unlockedCards.Contains(data))
            {
                Debug.Log($"Removing {data.name} from sorted cards");
                sortedCards.Remove(data);
            }
        }

        sortedCards.Sort((a, b) => a.cardTier.CompareTo(b.cardTier));

        if (sortedCards.Count > 0)
        {
            return sortedCards[0];
        }
        else
        {
            return null;
        }
    }
}
