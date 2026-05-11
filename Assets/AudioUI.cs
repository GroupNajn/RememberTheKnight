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
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        SoundsFXVolumeSlider.onValueChanged.AddListener(SetSoundFXVolume);

        masterVolumeSlider.value = 50;
        musicVolumeSlider.value = 50;
        SoundsFXVolumeSlider.value = 50;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolText.text = $"{(int)(volume)}";
        //AudioManager.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolText.text = $"{(int)(volume)}";
        //AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSoundFXVolume(float volume)
    {
        //AudioManager.Instance.SetSoundsFXVolume(volume);
        SoundsFXVolText.text = $"{(int)(volume)}";
    }




}
