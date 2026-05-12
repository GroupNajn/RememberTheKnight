using UnityEngine;
using FMODUnity;


public class PlayerSoundFXManager : CharacterSoundFXManager
{
    [Header("Player Specific Sound FX")]
    public EventReference FullyChargedEvent;
    //public EventReference Event;
    //public EventReference pickupEvent;

    protected override void Awake()
    {
        base.Awake();
    }
}
