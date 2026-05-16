using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioUI : AutoSelectFirstButtonOnEnable
{

    UIManager uiManager;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SoundsFXVolumeSlider;

    [SerializeField] TextMeshProUGUI masterVolText;
    [SerializeField] TextMeshProUGUI musicVolText;
    [SerializeField] TextMeshProUGUI SoundsFXVolText;

    private float baseMasterVolume;
    private float baseMusicVolume;
    private float baseSFXVolume;

    protected override void Start()
    {
        uiManager = GetComponentInParent<UIManager>();

        //masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        //musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        //SoundsFXVolumeSlider.onValueChanged.AddListener(SetSoundFXVolume);

        baseMasterVolume = WorldSoundFXManager.instance.GetMasterVolume();
        baseMusicVolume = WorldSoundFXManager.instance.GetMusicVolume();
        baseSFXVolume = WorldSoundFXManager.instance.GetSFXVolume();

        masterVolumeSlider.value = baseMasterVolume;
        musicVolumeSlider.value = baseMusicVolume;
        SoundsFXVolumeSlider.value = baseSFXVolume;

        base.Start();
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
