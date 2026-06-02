using FMOD;
using FMODUnity;
using UnityEngine;
using static IPickupable;

public class CharacterSoundFXManager : MonoBehaviour
{

    [Header("Death Sound")]
    public EventReference deathEvent;

    protected virtual void Awake()
    {
    }


    public virtual void PlayRollSoundFX()
    {
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.rollEvent, gameObject);

    }
    public virtual void PlayBackStepSoundFX()
    {
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.backstepEvent, gameObject);

    }


    public virtual void PlayDamageGrunt()
    {
        
            //  audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts), 0.6f);
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.damageEvent, gameObject);
        
    }
 
    public virtual void PlayDeathSoundFX()
    {
        RuntimeManager.PlayOneShotAttached(deathEvent, gameObject);
    }

    public virtual void PlayFootStep()
    {
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.enemyFootStepEvent, gameObject);
    }
   
}