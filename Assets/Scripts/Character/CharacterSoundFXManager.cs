using FMOD;
using FMODUnity;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    protected AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("Death Sounds")]
    [SerializeField] protected AudioClip[] deathSounds;


    [Header("FootSteps")]
    [SerializeField] protected AudioClip[] footSteps;
    // ADD WOOD public AudioClip[] footStepsWood;
    // ADD STONE 
    // ETC LATER

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        return;
        audioSource.PlayOneShot(soundFX, volume);

        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public virtual void PlayRollSoundFX()
    {
       // audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.rollEvent, gameObject);

        //  Debug.Log("Played roll sound effect");
    }
    public virtual void PlayBackStepSoundFX()
    {
       // audioSource.PlayOneShot(WorldSoundFXManager.instance.backstepSFX);
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.backstepEvent, gameObject);

    }
    public virtual void PlayPickUpSoundFX()
    {
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.pickUpSFX);
        RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.pickupEvent, gameObject);

    }

    public virtual void PlayDamageGrunt()
    {
        if (damageGrunts.Length > 0)
        {
            //  audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts), 0.6f);
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.damageEvent, gameObject);
        }
    }
    public virtual void PlayAttackGrunt()
    {
        if (attackGrunts.Length > 0)
        {
            //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts), 0.7f);

        }
    }
    public virtual void PlayDeathSoundFX()
    {
        if (deathSounds.Length > 0)
        {
            //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(deathSounds), 0.3f);
        }
    }

    public virtual void PlayFootStep()
    {
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.footStepEvent, gameObject);
    }
    public virtual void PlayAttackSwoosh()
    {
        if (footSteps.Length > 0)
        {
            RuntimeManager.PlayOneShotAttached(WorldSoundFXManager.instance.attackEvent, gameObject);

            // audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
        }
    }
}