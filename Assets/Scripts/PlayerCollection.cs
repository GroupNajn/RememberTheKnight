using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCollection : MonoBehaviour
{
    [SerializeField] private CardCollection cardCollection;
    private PlayerManager playerManager;
    private PlayerStats playerStats;
    float healthPercentage;

    [SerializeField] private List<CardData> displayEquipedCards = new List<CardData>();
    [SerializeField] private List<CardData> displayTempCards = new List<CardData>();

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
        Event_System.instance.OnConfirmCardSelection += EquipCard;
        Event_System.instance.OnLootPickedUp += PickupLoot;
        Event_System.instance.OnPlayerDeath += ClearTemporaryCards;
        Event_System.instance.OnConfirmPurchase += AddCardToTempOnPurchase;
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        ResetAllLists();
      

    }
    private void OnDestroy()
    {
        Event_System.instance.OnContractSign -= SignContract;
        Event_System.instance.OnConfirmCardSelection -= EquipCard;
        Event_System.instance.OnLootPickedUp -= PickupLoot;
        Event_System.instance.OnPlayerDeath -= ClearTemporaryCards;
        Event_System.instance.OnConfirmPurchase -= AddCardToTempOnPurchase;
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
        UpdateDisplayCollection();
    }

    public void AddCardToTempOnPurchase(List<CardData> cards)
    {
        healthPercentage = playerStats.CurrentHealth / playerStats.MaxHealth;
        foreach (CardData cardData in cards)
        {
            InsertIntoCardCollection(cardData);
        }
        playerManager.ReApplyStats();
        playerStats.CurrentHealth = playerStats.MaxHealth * healthPercentage;
        playerManager.Heal(0);
        UpdateDisplayCollection();

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
            healthPercentage = playerStats.CurrentHealth / playerStats.MaxHealth;
            InsertIntoCardCollection(card.CardData);
            playerManager.ReApplyStats();
            playerStats.CurrentHealth = playerStats.MaxHealth * healthPercentage;
            playerManager.Heal(0);
            UpdateDisplayCollection();
        }
    }

    private void ResetAllLists()
    {
        cardCollection.ClearEquipedCards();
        cardCollection.ClearTemporaryCards();
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
    public void UpdateDisplayCollection()
    {
        displayEquipedCards = cardCollection.GetEquippedCards();
        displayTempCards = cardCollection.GetTempCardCollection();
    }

    public bool CardIsPickedUp(CardData card)
    {
        var tempList = cardCollection.ReturnCardsForApplyingStats();

        foreach(CardData cardData in tempList)
        {
            if (card.cardID == cardData.cardID)
                return true;
        }
        return false;

    }
}
