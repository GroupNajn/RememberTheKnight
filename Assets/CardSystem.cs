using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class CardSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> allCards = new();
    [SerializeField] private List<CardData> unlockedCards = new();
    // Only to show in inspectorn and to store all the cards at start. 


    // To remove potential dupelettes. 
    private HashSet<CardData> hashUnlocked = new HashSet<CardData>();
    private HashSet<CardData> hashAllCards = new HashSet<CardData>();
    private CardContract cardContract;
    public CardContract PlayerConract
    {
        set => cardContract = value;
    }

    private int unlockedTier = (int)Tier.I;
    private int unlockableTier = (int)Tier.III;

    void Start()
    {
        InitializeHashSets();
        //UnlockAllTierOneToThreeTemporary();


    }
    void Update()
    {

    }

    public IReadOnlyList<CardData> GetUnlockedCards()
    {
        return unlockedCards;
    }

    public IReadOnlyList<CardData> GetAllCards()
    {
        return allCards;
    }

    //Temporary Method to return a randomCardData in the allcards list. 
    public CardData ReturnRandomCard()
    {

        int index = Random.Range(0, allCards.Count);
        return allCards[index];
    }

    //Method to be called after a contract is signed and is no longer null.
    // To set the Unlocked Cards at start. 
    public void UnlockCardsAfterSigningContract(CardContract contract)
    {
        foreach (CardData cardData in allCards)
        {
            if (cardData.cardFamily == contract.CardFamily && (int)cardData.cardTier <= unlockableTier)
            {
                unlockedCards.Add(cardData);
                hashUnlocked.Add(cardData);

            }
        }

        Debug.Log($"UNLOCKED CARDS: {unlockedCards.Count}");

        foreach (CardData cardData in hashUnlocked)
        {
            Debug.Log($"Card ID: {cardData.cardID}");
        }
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
        Debug.Log($"Antal Kort i unlocked List:  {unlockedCards.Count}");
    }

    private void InitializeHashSets()
    {
        foreach (CardData card in allCards)
        {
            hashAllCards.Add(card);
        }
    }









}
