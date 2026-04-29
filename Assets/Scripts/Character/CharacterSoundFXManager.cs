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
        audioSource.PlayOneShot(soundFX, volume);

        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public virtual void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        Debug.Log("Played roll sound effect");
    }
    public virtual void PlayBackStepSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.backstepSFX);
    }
    public virtual void PlayPickUpSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.pickUpSFX);
    }

    public virtual void PlayDamageGrunt()
    {
        if (damageGrunts.Length > 0)
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts), 0.6f);
        }
    }
    public virtual void PlayAttackGrunt()
    {
        if (damageGrunts.Length > 0)
        {  
            audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts), 0.7f);
        }
    }
    public virtual void PlayDeathSoundFX()
    {
        if (deathSounds.Length > 0)
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(deathSounds), 0.3f);
        }
    }

    public virtual void PlayFootSteps()
    {
        if (footSteps.Length > 0)
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
        }
    }
}
