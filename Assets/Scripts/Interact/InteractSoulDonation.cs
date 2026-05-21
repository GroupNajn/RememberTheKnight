using FMODUnity;
using UnityEditor.Searcher;
using UnityEngine;

public class InteractSoulDonation : MonoBehaviour, IInteractable, IInteractableUIText
{
    Loot_System lootSystem;
    CardSystem cardSystem;
    PlayerCollection playerCollection;
    private DonationMoveSoul moveSoul;
    bool NewCardUnlocked { get; set; } = false;

    CardData nextCard;
    int soulsRequired;
    [SerializeField] int soulsDonated;
    private int soulsDonatedForNextUnlock;
    private int donateAmount = 1;

    public EventReference donateEvent;
    public EventReference unlockCardEvent;


    private void Start()
    {
        lootSystem = LootManager.instance.gameObject.GetComponent<Loot_System>();
        cardSystem = LootManager.instance.gameObject.GetComponentInChildren<CardSystem>();
        playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        nextCard = cardSystem.GetNextCardInSelectedFamily();
        moveSoul = GetComponent<DonationMoveSoul>();
        SetSoulsDonatedSinceLast();
        if (nextCard)
        {
            soulsRequired = (int)Mathf.Pow(nextCard.cardSoulCost, 2);
            soulsDonatedForNextUnlock = (int)Mathf.Pow(nextCard.cardSoulCost, 2);
            NewCardUnlocked = true;
        }
    }

    private void OnDestroy()
    {
        SetSoulsToNextCardGlobally(soulsDonated);
    }

    private void SetSoulsGlobally(int amount)
    {
        GameObject.Find("GlobalData").GetComponent<GameData>().SoulsDoantedSinceLast += amount;
    }

    private void SetSoulsToNextCardGlobally(int donationAmount)
    {
        GameObject.Find("GlobalData").GetComponent<GameData>().SoulsRemainingToNextUnlock = (soulsDonatedForNextUnlock -= donationAmount);
    }

    private void ResetSoulsGlobally()
    {
        GameObject.Find("GlobalData").GetComponent<GameData>().SoulsDoantedSinceLast = 0;
    }

    private void SetSoulsDonatedSinceLast()
    {
        soulsDonated = GameObject.Find("GlobalData").GetComponent<GameData>().SoulsDoantedSinceLast;
    }

    public bool SetNextCard()
    {
        return (nextCard = cardSystem.GetNextCardInSelectedFamily()) != null;
    }

    public int SetNetCardCost() => soulsRequired = (int)Mathf.Pow((float)nextCard.cardSoulCost, 2);
    public int SetSoulnsDonateForNextUnlock() => soulsDonatedForNextUnlock = (int)Mathf.Pow((float)nextCard.cardSoulCost, 2);

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        if (playerCollection.playerContract != null)
        {
            UIData.InfoText = "[F]: Donate Souls";
        }
        else if (playerCollection.playerContract != null && lootSystem.currentSoulCount <= 0)
        {
            UIData.InfoText = "[F]: Not enough souls.";
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
            //Debug.Log("No Family Selected, can not donate");
            //Debug.Log("Family signed: " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract.signed.ToString());
            //return;
        }



        if (lootSystem.currentSoulCount > 0 && soulsDonated < soulsRequired && nextCard != null)
        {
            RuntimeManager.StudioSystem.setParameterByName("CardUnlock", (float)soulsDonated / (float)soulsRequired);
            RuntimeManager.PlayOneShotAttached(donateEvent, gameObject);

            lootSystem.ConsumeSouls(donateAmount);
            moveSoul.InstantiateSoul();
            SetSoulsGlobally(donateAmount);
            
            soulsDonated++;
            if (soulsDonated >= soulsRequired)
            {
                    NewCardUnlocked = true;
                soulsDonated = 0;
                SetSoulsGlobally(soulsDonated);
                ResetSoulsGlobally();
                RuntimeManager.PlayOneShotAttached(unlockCardEvent, gameObject);


                if (nextCard != null)
                {
                    Event_System.instance.OnSacrificeSuccessful?.Invoke(nextCard);
                }
            }
        }
    }
}