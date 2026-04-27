using UnityEngine;

public class Card : Loot, IPickupable
{
    [Header("Card Data")]
    [field: SerializeField] protected CardData cardData { get; private set; }
    public CardData CardData => cardData;

    private CardUnlockType cardUnlockType = CardUnlockType.Permanent;

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
        //Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player") && pickable == PickableState.Pickable)
        {
            Pickup();
        }
    }

    public override void Pickup()
    {
        //Debug.Log($"You picked up {itemName}");

        Destroy(gameObject);
        Event_System.instance?.OnLootPickedUp.Invoke(this);
    }

   
}
