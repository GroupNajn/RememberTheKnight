using FMODUnity;
using UnityEngine;

public class ChargedSoul : Soul, IPickupable
{

    public override void Pickup()
    {
        RuntimeManager.PlayOneShotAttached(soulPickupEvent, playerTransform.gameObject);

        base.Pickup();
    }
}
