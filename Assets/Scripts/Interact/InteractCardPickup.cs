using UnityEngine;

public class InteractCardPickup : MonoBehaviour, IInteractable, IInteractableUIText
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CardData cardData;
    private Card card;
    void Start()
    {
        card = GetComponent<Card>();
        cardData = card.CardData;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Interact()
    {
        card.Pickup();
    }

    public InteractableUIData GetUIData()
    {
        var data = new InteractableUIData();
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();

        if (cardData == null) return null;

        if (collection.CardIsPickedUp(cardData))
        {
            data.InfoText = $"[F]: {cardData.cardName} is already picked up.";
            data.ErrorText = $"{cardData.cardName} will be sacrificed and you will gain souls.";
            data.CanInteract = true;
        }
        else if (!collection.CardIsPickedUp(cardData))
        {
            data.InfoText = $"[F]: Pickup: {cardData.cardName}.";
            data.CanInteract = true;
        }
        else
            data.InfoText = $"{cardData.name}";
        return data;



        
    }


}
