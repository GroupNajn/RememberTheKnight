using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : AutoSelectFirstButtonOnEnable
{

    UIManager uiManager;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SoundsFXVolumeSlider;

    [SerializeField] TextMeshProUGUI masterVolText;
    [SerializeField] TextMeshProUGUI musicVolText;
    [SerializeField] TextMeshProUGUI SoundsFXVolText;

    private float baseMasterVolume = 0.75f;
    private float baseMusicVolume = 0.75f;
    private float baseSFXVolume = 0.75f;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        uiManager = GetComponentInParent<UIManager>();

        // VILL VI BÖRJA PÅ 100% UNCOMMENT DETTA
        //baseMasterVolume = WorldSoundFXManager.instance.GetMasterVolume();
        //baseMusicVolume = WorldSoundFXManager.instance.GetMusicVolume();
        //baseSFXVolume = WorldSoundFXManager.instance.GetSFXVolume();

        masterVolumeSlider.value = baseMasterVolume;
        musicVolumeSlider.value = baseMusicVolume;
        SoundsFXVolumeSlider.value = baseSFXVolume;

    }

    public void SetMasterVolume(float volume)
    {
        masterVolText.text = $"{(int)(volume * 100)}";
        WorldSoundFXManager.instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolText.text = $"{(int)(volume * 100)}";
        WorldSoundFXManager.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundsFXVolText.text = $"{(int)(volume * 100)}";
        WorldSoundFXManager.instance.SetSFXVolume(volume);
    }

}
