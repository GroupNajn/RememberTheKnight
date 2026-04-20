using System.Collections;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Damage Sounds")]
    public AudioClip[] damageSFX;

    [Header("Action Sounds")]
    public AudioClip rollSFX;
    public AudioClip backstepSFX;
    public AudioClip pickUpSFX;

    [Header("Boss Music")]
    [SerializeField] AudioSource BossIntroPlayer;
    [SerializeField] AudioSource BossLoopPlayer;

    [Header("Background Music")]
    [SerializeField] AudioSource BackgroundIntroPlayer;
    [SerializeField] AudioSource BackgroundLoopPlayer;


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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBackgroundTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        BackgroundIntroPlayer.volume = 0.5f;
        BackgroundIntroPlayer.clip = introTrack;
        BackgroundIntroPlayer.loop = false;
        BackgroundIntroPlayer.Play();

        BackgroundLoopPlayer.volume = 0.5f;
        BackgroundLoopPlayer.clip = loopTrack;
        BackgroundLoopPlayer.loop = true;
        BackgroundLoopPlayer.PlayDelayed(BackgroundIntroPlayer.clip.length);
    }

    public void StopBackgroundMusic()
    {
        StartCoroutine(FadeOutBackgroundMusicThenStop());
    }

    private IEnumerator FadeOutBackgroundMusicThenStop()
    {
        while (BackgroundLoopPlayer.volume > 0)
        {
            BackgroundIntroPlayer.volume -= Time.deltaTime;
            BackgroundLoopPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        BackgroundIntroPlayer.Stop();
        BackgroundLoopPlayer.Stop();
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);

        return array[index];
    }







}
