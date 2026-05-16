using FMODUnity;
using UnityEngine;

public class InteractSoulDonation : MonoBehaviour, IInteractable, IInteractableUIText
{
    Loot_System lootSystem;
    CardSystem cardSystem;
    PlayerCollection playerCollection;

    CardData nextCard;
    int soulsRequired;
    int soulsDonated;

    public EventReference donateEvent;
    public EventReference unlockCardEvent;


    private void Start()
    {
        lootSystem = LootManager.instance.gameObject.GetComponent<Loot_System>();
        cardSystem = LootManager.instance.gameObject.GetComponentInChildren<CardSystem>();
        playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        nextCard = cardSystem.GetNextCardInSelectedFamily();
        if (nextCard)
        {
            soulsRequired = (int)nextCard.cardSoulCost;
        }
    }

    public bool SetNextCard()
    {
        return (nextCard = cardSystem.GetNextCardInSelectedFamily()) != null;
    }

    public int SetNetCardCost() => soulsRequired = (int)Mathf.Pow((float)nextCard.cardSoulCost, 2);

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        if (playerCollection.playerContract != null)
        {
            UIData.InfoText = "[F]: Donate Souls";

        }
        else if (playerCollection.playerContract == null)
        {
            UIData.InfoText = "[F]: You do not have a signed Contract";
            UIData.ErrorText = "Go to lobby to sign a contract;";
        }

        return UIData;
    }

    public void Interact()
    {
        // Add dialogue if wanted
        if (playerCollection.playerContract == null) return;
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


                if (nextCard != null)
                {
                    Event_System.instance.OnSacrificeSuccessful?.Invoke(nextCard);
                }
            }
        }
    }
}