using UnityEngine;

public class InteractSoulDonation : MonoBehaviour, IInteractable, IInteractableUIText
{
    Loot_System lootSystem;
    CardSystem cardSystem;

    CardData nextCard;
    int soulsRequired;
    int soulsDonated;

    private void Start()
    {
        lootSystem = LootManager.instance.gameObject.GetComponent<Loot_System>();
        cardSystem = LootManager.instance.gameObject.GetComponentInChildren<CardSystem>();

        nextCard = cardSystem.GetNextCardInSelectedFamily();
        if (nextCard)
        {
            soulsRequired = (int)nextCard.cardSoulCost;
        }
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "[F]: Donate Souls";

        return UIData;
    }

    public void Interact()
    {
        // Add dialogue if wanted

        if (lootSystem.currentSoulCount > 0 && soulsDonated < soulsRequired)
        {
            lootSystem.ConsumeSouls(1);
            soulsDonated++;
            if (soulsDonated >= soulsRequired)
            {
                Debug.Log($"Unlocked card {nextCard.name}");
                cardSystem.UnlockCardFromDonation(nextCard);
            }
        }
    }
}