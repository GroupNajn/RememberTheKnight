using FMODUnity;
using UnityEngine;

public class Soul: Loot, IPickupable
{
    [field: SerializeField] public int SoulCollectReward { get; private set; } = 1;

    public EventReference soulBounceEvent;
    

    protected override void Start()
    {
        base.Start();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player") && pickable == PickableState.Pickable)
        {

            Pickup();
        }
    }
    
}
