using FMODUnity;
using System.Collections;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.Rendering;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sounds")]
   // public AudioClip[] damageSFX;
    public EventReference damageEvent;

    [Header("Action Sounds")]
   // public AudioClip rollSFX;
    //public AudioClip backstepSFX;
    //public AudioClip pickUpSFX;
    public EventReference rollEvent;
    public EventReference backstepEvent;
    public EventReference cardPickupEvent;
    public EventReference attackEvent;
    public EventReference footStepEvent;
    public EventReference teleportEvent;
    public EventReference playerWakeUpEvent;
    public EventReference explosionEvent;
    public EventReference fireLoopEvent;
    public EventReference shopBuyCardEvent;
    public EventReference shopSelectCardEvent;
    public EventReference errorEvent;
    public EventReference shopDeselectCardEvent;


    [Header("Button Sounds")]
    public EventReference cardFlipEvent;
    public EventReference bookOpenEvent;
    public EventReference bookCloseEvent;
    public EventReference bookPageFlipEvent;
    public EventReference bookSlideEvent;

    [Header("VCA")]
    private VCA masterVCA;
    private VCA musicVCA;
    private VCA sfxVCA;

    //[Header("Boss Music")]
    //[SerializeField] AudioSource BossIntroPlayer;
    //[SerializeField] AudioSource BossLoopPlayer;

    //[Header("Background Music")]
    // [SerializeField] AudioSource BackgroundIntroPlayer;
    //  [SerializeField] AudioSource BackgroundLoopPlayer;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        try
        {
            masterVCA = RuntimeManager.GetVCA("vca:/Master");
            musicVCA = RuntimeManager.GetVCA("vca:/Music");
            sfxVCA = RuntimeManager.GetVCA("vca:/SFX");
        }
        catch
        {
            Debug.Log("Fatal error prevented when setting vca references");
        }
       
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public float GetMasterVolume()
    {
        masterVCA.getVolume(out float volume);

        return volume;
    }
    public float GetMusicVolume()
    {
        musicVCA.getVolume(out float volume);

        return volume;
    }
    public float GetSFXVolume()
    {
        sfxVCA.getVolume(out float volume);

        return volume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVCA.setVolume(volume);
    }
    public void SetMusicVolume(float volume)
    {
        musicVCA.setVolume(volume);
    }
    public void SetSFXVolume(float volume)
    {
        sfxVCA.setVolume(volume);
    }

    //public void PlayBackgroundTrack(AudioClip introTrack, AudioClip loopTrack)
    //{
    //    BackgroundIntroPlayer.volume = 0.5f;
    //    BackgroundIntroPlayer.clip = introTrack;
    //    BackgroundIntroPlayer.loop = false;
    //    BackgroundIntroPlayer.Play();

    //    BackgroundLoopPlayer.volume = 0.5f;
    //    BackgroundLoopPlayer.clip = loopTrack;
    //    BackgroundLoopPlayer.loop = true;
    //    BackgroundLoopPlayer.PlayDelayed(BackgroundIntroPlayer.clip.length);
    //}

    //public void StopBackgroundMusic()
    //{
    //    StartCoroutine(FadeOutBackgroundMusicThenStop());
    //}

    //private IEnumerator FadeOutBackgroundMusicThenStop()
    //{
    //    while (BackgroundLoopPlayer.volume > 0)
    //    {
    //        BackgroundIntroPlayer.volume -= Time.deltaTime;
    //        BackgroundLoopPlayer.volume -= Time.deltaTime;
    //        yield return null;
    //    }

    //    BackgroundIntroPlayer.Stop();
    //    BackgroundLoopPlayer.Stop();
    //}

    //public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    //{
    //    int index = Random.Range(0, array.Length);

    //    return array[index];
    //}
}