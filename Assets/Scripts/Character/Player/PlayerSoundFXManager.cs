using UnityEngine;
using FMODUnity;


public class PlayerSoundFXManager : CharacterSoundFXManager
{
    [Header("Player Specific Sound FX")]
    public EventReference FullyChargedEvent;
    public EventReference outOfBreathEvent;
    public EventReference lowHealthEvent;

    //public EventReference Event;
    //public EventReference pickupEvent;

    protected override void Awake()
    {
        base.Awake();
    }
    protected void Start()
    {
        RuntimeManager.PlayOneShotAttached(outOfBreathEvent, gameObject); // out of breath sound effect
        RuntimeManager.PlayOneShotAttached(lowHealthEvent, gameObject); // out of breath sound effect
    }

    public override void PlayFootStep()
    {
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.footStepEvent, gameObject);
    }
}
