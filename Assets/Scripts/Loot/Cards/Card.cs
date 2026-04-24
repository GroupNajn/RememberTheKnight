using UnityEngine;

public class Card : Loot, IPickupable
{
    [Header("Card Data")]
    [field: SerializeField] protected ScriptableObject lootData { get; private set; }

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
