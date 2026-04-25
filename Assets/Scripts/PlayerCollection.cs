using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;

public class PlayerCollection : MonoBehaviour
{
    [SerializeField] private CardCollection cardCollection;
    
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
        cardCollection.AddToCollection(card);
    }
    public List<Card> ReturnTempCardCollection()
    {
        return cardCollection.GetTempCardCollection();
    }

    public List<Card> ReturnPermanentCardCollection()
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
