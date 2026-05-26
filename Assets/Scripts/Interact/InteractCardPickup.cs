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
        data.CanInteract = true;
        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();

        if (cardData == null) return null;

        if (collection.CardIsPickedUp(cardData))
        {
            data.InfoText = $"{cardData.cardName} is already picked up.";
            data.ErrorText = $"{cardData.cardName} will be sacrificed and you will gain souls.";
        }
        else if (!collection.CardIsPickedUp(cardData))
        {
            data.InfoText = $"Pickup: {cardData.cardName}.";
        }
        else
            data.InfoText = $"{cardData.name}";

        return data;
    }


}
