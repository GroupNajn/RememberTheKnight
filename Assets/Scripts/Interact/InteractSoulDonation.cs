using FMODUnity;
using UnityEngine;

public class InteractSoulDonation : MonoBehaviour, IInteractable, IInteractableUIText
{
    Loot_System lootSystem;
    CardSystem cardSystem;
    PlayerCollection playerCollection;
    private DonationMoveSoul moveSoul;
    private CardContract currentContract;
    private CardContract previousContract;
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
        currentContract = playerCollection.playerContract;
        nextCard = cardSystem.GetNextCardInSelectedFamily();
        moveSoul = GetComponent<DonationMoveSoul>();
        SetSoulsDonatedSinceLastToFamily();

        if (nextCard)
        {
            soulsRequired = GameDataUpdater.instance.GetSoulCostForIngameRun(playerCollection.playerContract);
            soulsDonatedForNextUnlock = (int)Mathf.Pow(nextCard.cardSoulCost, 2);
            NewCardUnlocked = true;
        }
    }

    private void OnDestroy()
    {
        GameDataUpdater.instance.SetSoulsCostInNextCard(playerCollection.playerContract);
        GameDataUpdater.instance.SetSoulsRemaingToNextUnlock(playerCollection.playerContract, soulsDonated);
        SetSoulsDonatedSinceLastToFamily();
    }


    public void SetSoulsDonatedSinceLastToFamily()
    {
        GameDataUpdater.instance.SetSoulsDonatedSinceLastToFamily(playerCollection.playerContract, soulsDonated);
    }


  

  

    //private void ResetSoulsGlobally()
    //{
    //    GameObject.Find("GlobalData").GetComponent<GameData>().soulsDonatedSinceLast = 0;
    //}

    //private void SetSoulsDonatedSinceLast()
    //{
    //    soulsDonated = GameObject.Find("GlobalData").GetComponent<GameData>().soulsDonatedSinceLast;
    //}

    public bool SetNextCard()
    {
        return (nextCard = cardSystem.GetNextCardInSelectedFamily()) != null;
    }

    public int SetNextCardCost() => soulsRequired = (int)Mathf.Pow((float)nextCard.cardSoulCost, 2);
    public int SetSoulnsDonateForNextUnlock() => soulsDonatedForNextUnlock = (int)Mathf.Pow((float)nextCard.cardSoulCost, 2);

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        if (playerCollection.playerContract != null)
        {
            UIData.InfoText = "Donate Souls";
        }
        else if (playerCollection.playerContract != null && lootSystem.currentSoulCount <= 0)
        {
            UIData.CanInteract = false;
            UIData.InfoText = "Not enough souls.";
        }
        else if (playerCollection.playerContract == null)
        {
            UIData.CanInteract = false;
            UIData.InfoText = "You do not have a signed Contract";
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
            //SetSoulsGlobally(donateAmount);
            
            soulsDonated++;
            if (soulsDonated >= soulsRequired)
            {
                    NewCardUnlocked = true;
                soulsDonated = 0;
                //SetSoulsGlobally(soulsDonated);
                //ResetSoulsGlobally();
                RuntimeManager.PlayOneShotAttached(unlockCardEvent, gameObject);


                if (nextCard != null)
                {
                    Event_System.instance.OnSacrificeSuccessful?.Invoke(nextCard);
                }
            }
        }
    }
}