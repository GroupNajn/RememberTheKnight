using UnityEngine;

public class Soul: Loot, IPickupable
{


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
