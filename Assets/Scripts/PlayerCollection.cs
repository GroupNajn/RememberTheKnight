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

    }

    public void InsertIntoCardSelectin(Card card)
    {
        cardCollection.AddToCollection(card.CardData);
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
        cardContract = new CardContract(cardFamily);
        if (cardContract != null)
            cardContract.SignContract();
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
