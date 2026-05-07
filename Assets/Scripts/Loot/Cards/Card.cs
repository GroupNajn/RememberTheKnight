using System.Runtime.CompilerServices;
using UnityEngine;

public class Card : Loot, IPickupable
{
    [Header("Card Data")]
    [field: SerializeField] protected CardData cardData { get; private set; }
    public CardData CardData => cardData;


    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    public GameObject CardFront
    {
        get => cardFront;
        set => cardFront = value;
    }
    public GameObject CardBack
    {
        get => cardBack;
        set => cardBack = value;
    }

    private bool hasBeenPickedUp = false;
    public CardData SetCardData(CardData card) => cardData = card; 

    private CardUnlockType cardUnlockType = CardUnlockType.Permanent;


    protected override void  Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            playerTransform = player.transform;

        StartCoroutine(WaitForInitialization(pickUpDelay));
        if (LootManager.instance != null)
            LootManager.instance.RegisterLoot(this);

    }

    



    public CardUnlockType CardUnlockType
    {
        get => cardUnlockType;
        set => cardUnlockType = value;
    }

    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (hasBeenPickedUp)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (pickable != PickableState.Pickable)
            return;

        hasBeenPickedUp = true;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Pickup();
    }


}
