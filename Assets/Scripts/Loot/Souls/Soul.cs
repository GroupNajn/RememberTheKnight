using FMODUnity;
using UnityEngine;

public class Soul: Loot, IPickupable
{
    [field: SerializeField] public int SoulCollectReward { get; private set; } = 1;

    public EventReference soulBounceEvent;
    public EventReference soulAppearEvent;
    public EventReference soulPickupEvent;



    protected override void Start()
    {
        base.Start();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        RuntimeManager.PlayOneShotAttached(soulAppearEvent, gameObject);
        if (other.CompareTag("Player") && pickable == PickableState.Pickable)
        {
            Pickup();
        }
    }

    public override void Pickup()
    {
        RuntimeManager.PlayOneShotAttached(soulPickupEvent, playerTransform.gameObject);

        base.Pickup();
    }
    
}
