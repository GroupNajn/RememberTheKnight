using FMODUnity;
using UnityEngine;

public class BossSoundEffectsManager : CharacterSoundFXManager
{
    public override void PlayFootStep()
    {
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.bossFootStepEvent, gameObject);
    }
}
