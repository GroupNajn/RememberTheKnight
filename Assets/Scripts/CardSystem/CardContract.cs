using System.Diagnostics.Contracts;
using UnityEngine;

public class CardContract
{
    private CardFamily cardFamily = CardFamily.None;

    public CardFamily CardFamily => cardFamily;

    public enum Signed { Not, Signed}

    public Signed signed { get; set; }
    public CardContract(CardFamily cardFamily)
    {
        this.cardFamily = cardFamily;
        
    }

    public void BreakContract()
    {
        signed = Signed.Not;
    }

    public void SignContract()
    {
        signed = Signed.Signed;
    }

    public CardContract GetCardContract()
    {
        if (this == null) return null;
        return this;
    }
    


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
