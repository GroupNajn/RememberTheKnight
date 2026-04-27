using UnityEngine;

public class CardContract : MonoBehaviour
{
    private CardFamily cardFamily = CardFamily.None;

    public CardFamily CardFamily => cardFamily;

    public enum Signed { Not, Signed }
    public Signed signed
    {
        get => signed;
        private set => signed = value;
    }
    public CardContract(CardFamily cardFamily)
    {
        this.cardFamily = cardFamily;
        signed = Signed.Signed;
    }

    public void BreakContract()
    {
        signed = Signed.Not;
    }

    public void SignContract()
    {
        signed = Signed.Signed;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
