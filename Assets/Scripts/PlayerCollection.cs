using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCollection : MonoBehaviour
{
    [SerializeField] private CardCollection cardCollection;
    private PlayerManager playerManager;

    public CardCollection CardCollection
    {
        get => cardCollection;
    }

    private CardContract cardContract;

    public CardContract playerContract
    {
        get => cardContract;
    }
    void Start()
    {
        Event_System.instance.OnContractSign += SignContract;
        Event_System.instance.OnStatsApplied += EquipCard;
        Event_System.instance.OnLootPickedUp += PickupLoot;
        Event_System.instance.OnPlayerDeath += ClearTemporaryCards;
        playerManager = GetComponent<PlayerManager>();
    }
    private void OnDestroy()
    {
        Event_System.instance.OnContractSign -= SignContract;
        Event_System.instance.OnStatsApplied -= EquipCard;
        Event_System.instance.OnLootPickedUp -= PickupLoot;
        Event_System.instance.OnPlayerDeath -= ClearTemporaryCards;
    }

    public void InsertIntoCardCollection(CardData card)
    {
        cardCollection.AddToTempCollection(card);
    }

    public void EquipCard(List<CardData> cards)
    {
        //Clear Equiped Cards so the stats does not stack.
        cardCollection.ClearEquipedCards();
        foreach (CardData cardData in cards)
        {
            cardCollection.EquipCards(cardData);
        }

        //where the stats gets applyed the playerStats
        playerManager.ApplyStatsFromCardSelection(cardCollection.ReturnCardsForApplyingStats());
    }


    public void ReciveSelectedCardList(List<CardData> cards)
    {
         
        foreach (CardData card in cards)
        {
            InsertIntoCardCollection(card);
        }
      
    }
    public List<CardData> ReturnTempCardCollection()
    {
        return cardCollection.GetTempCardCollection();
    }

    public List<CardData> ReturnPermanentCardCollection()
    {
        return cardCollection.GetEquippedCards();
    }

    public List<CardData> ReturnAllCards()
    {
        return cardCollection.ReturnCardsForApplyingStats();
    }

    public void SignContract(CardFamily cardFamily)
    {

        if (cardContract != null) return;


        cardContract = new CardContract(cardFamily);
        if (cardContract != null)
        {
            cardContract.SignContract();
            cardCollection.PlayersCardContract = cardContract;
            CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
            cardSystem.PlayerConract = cardContract;

        }
    }

    public void PickupLoot(Loot loot)
    {
        if (loot is Card card)
        {
            InsertIntoCardCollection(card.CardData);
            playerManager.ApplyStatsInternally(card.CardData);

        }

    }


    public void ClearTemporaryCards()
    {
        cardCollection.ClearTemporaryCards();
    }

    public void BreakContract()
    {
        if (cardContract != null)
            cardContract.BreakContract();
    }

    void Update()
    {

    }
}
