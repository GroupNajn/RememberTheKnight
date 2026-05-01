using UnityEngine;

public class Soul: Loot, IPickupable
{

    


    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player") && pickable == PickableState.Pickable)
        {
            Pickup();
        }
    }
    
}
