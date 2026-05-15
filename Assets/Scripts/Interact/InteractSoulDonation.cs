using FMODUnity;
using UnityEditor.EditorTools;
using UnityEngine;

public class InteractSoulDonation : MonoBehaviour, IInteractable, IInteractableUIText
{
    Loot_System lootSystem;
    CardSystem cardSystem;

    CardData nextCard;
    int soulsRequired;
    int soulsDonated;

    public EventReference donateEvent;
    public EventReference unlockCardEvent;


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
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract.signed == CardContract.Signed.Not)
        {
            Debug.Log("No Family Selected, can not donate");
            Debug.Log("Family signed: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract.signed.ToString());
            return;
        }



        if (lootSystem.currentSoulCount > 0 && soulsDonated < soulsRequired && nextCard != null)
        {
            RuntimeManager.StudioSystem.setParameterByName("CardUnlock", soulsDonated / soulsRequired);
            RuntimeManager.PlayOneShotAttached(donateEvent, gameObject);

            lootSystem.ConsumeSouls(1);
            soulsDonated++;
            if (soulsDonated >= soulsRequired)
            {
                soulsDonated = 0;
                RuntimeManager.PlayOneShotAttached(unlockCardEvent, gameObject);


                nextCard = cardSystem.GetNextCardInSelectedFamily();
                if (nextCard != null)
                {
                    Event_System.instance.OnSacrificeSuccessful?.Invoke(nextCard);
                    soulsRequired = (int)nextCard.cardSoulCost;
                }
            }
        }
    }
}