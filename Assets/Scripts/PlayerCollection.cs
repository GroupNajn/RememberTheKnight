using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCollection : MonoBehaviour
{
    [SerializeField] private CardCollection cardCollection;

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
        Event_System.instance.OnLootPickedUp += PickupLoot;
    }
    private void OnDestroy()
    {
        Event_System.instance.OnContractSign -= SignContract;
        Event_System.instance.OnLootPickedUp -= PickupLoot;
    }

    public void InsertIntoCardCollection(CardData card)
    {
        cardCollection.AddToCollection(card);
    }
    public void ReciveSelectedCardList(List<CardData> cards)
    {
        foreach(CardData card in cards)
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
        return cardCollection.GetPermanentCardCollection();
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
        if(loot is Card card)
        {
            InsertIntoCardCollection(card.CardData);

        } 

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
