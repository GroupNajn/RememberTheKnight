using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class VideoUI : AutoSelectFirstButtonOnEnable

{
    UIManager uiManager;
    [SerializeField] Slider bloomSlider;
    [SerializeField] Slider motionSlider;
    [SerializeField] Slider filmSlider;

    [SerializeField] TextMeshProUGUI bloomText;
    [SerializeField] TextMeshProUGUI motionText;
    [SerializeField] TextMeshProUGUI filmText;

    [SerializeField] Toggle bloomToggle;
    [SerializeField] Toggle motionToggle;
    [SerializeField] Toggle filmToggle;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullScreenToggle;

    private List<Resolution> filteredResolutions = new List<Resolution>();

    float lastBloomValue;
    float lastMotionValue;
    float lastFilmValue;

    float baseBloomValue;
    float baseMotionValue;
    float baseFilmValue;

    bool sliderInput = false;

    private const string bloomKey = "BloomIntensity";
    private const string motionKey = "MotionBlurIntensity";
    private const string filmKey = "FilmGrainIntensity";

    private const string bloomEnabledKey = "BloomEnabled";
    private const string motionEnabledKey = "MotionBlurEnabled";
    private const string filmEnabledKey = "FilmGrainEnabled";

    private const string resolutionKey = "ResolutionIndex";
    private const string fullScreenKey = "FullScreen";

    bool isLoadingSettings = false;

    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        uiManager = GetComponentInParent<UIManager>();

        isLoadingSettings = true;

        SetupResolutionDropdown();

        float bloomValue = PlayerPrefs.GetFloat(bloomKey, GlobalVolumeManager.Instance.GetBloomIntensity());
        float motionValue = PlayerPrefs.GetFloat(motionKey, GlobalVolumeManager.Instance.GetMotionBlurIntensity());
        float filmValue = PlayerPrefs.GetFloat(filmKey, GlobalVolumeManager.Instance.GetFilmGrainIntensity());

        bool bloomEnabled = PlayerPrefs.GetInt(bloomEnabledKey, bloomValue > 0 ? 1 : 0) == 1;
        bool motionEnabled = PlayerPrefs.GetInt(motionEnabledKey, motionValue > 0 ? 1 : 0) == 1;
        bool filmEnabled = PlayerPrefs.GetInt(filmEnabledKey, filmValue > 0 ? 1 : 0) == 1;

        lastBloomValue = bloomValue > 0 ? bloomValue : 0.75f;
        lastMotionValue = motionValue > 0 ? motionValue : 0.75f;
        lastFilmValue = filmValue > 0 ? filmValue : 0.75f;

        bloomSlider.value = bloomEnabled ? bloomValue : 0;
        motionSlider.value = motionEnabled ? motionValue : 0;
        filmSlider.value = filmEnabled ? filmValue : 0;

        SetBloomSlider(bloomSlider.value);
        SetMotionSlider(motionSlider.value);
        SetFilmSlider(filmSlider.value);

        GlobalVolumeManager.Instance.EnableBloom(bloomEnabled);
        GlobalVolumeManager.Instance.EnableMotionBlur(motionEnabled);
        GlobalVolumeManager.Instance.EnableFilmGrain(filmEnabled);

        baseBloomValue = bloomSlider.value;
        baseMotionValue = motionSlider.value;
        baseFilmValue = filmSlider.value;

        int savedResolutionIndex = PlayerPrefs.GetInt(resolutionKey, resolutionDropdown.value);

        if (savedResolutionIndex >= 0 && savedResolutionIndex < resolutionDropdown.options.Count)
        {
            resolutionDropdown.value = savedResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(savedResolutionIndex);
        }

        isLoadingSettings = false;
    }

    // SLIDERS
    public void SetBloomSlider(float value)
    {
        bloomText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        bloomToggle.isOn = enabled;

        //bloomText.color = new Color(bloomText.color.r, bloomText.color.g, bloomText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetBloomIntensity(value);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetFloat(bloomKey, value);
        PlayerPrefs.SetInt(bloomEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetMotionSlider(float value)
    {
        motionText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        motionToggle.isOn = enabled;

        //motionText.color = new Color(motionText.color.r, motionText.color.g, motionText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetMotionBlurIntensity(value);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetFloat(motionKey, value);
        PlayerPrefs.SetInt(motionEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.Save();

    }

    public void SetFilmSlider(float value)
    {
        filmText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        filmToggle.isOn = enabled;

        //filmText.color = new Color(filmText.color.r, filmText.color.g, filmText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetFilmGrainIntensity(value);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetFloat(filmKey, value);
        PlayerPrefs.SetInt(filmEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    // TOGGLES
    public void SetBloomToggle()
    {
        bool enabled = bloomToggle.isOn;

        if (!enabled)
        {
            if (bloomSlider.value > 0)
                lastBloomValue = bloomSlider.value;

            bloomSlider.value = 0;
        }
        else
        {
            bloomSlider.value = lastBloomValue;

        }

        bloomText.color = new Color(bloomText.color.r, bloomText.color.g, bloomText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.EnableBloom(enabled);
        GlobalVolumeManager.Instance.EnableLensFlare(enabled);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetInt(bloomEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.SetFloat(bloomKey, enabled ? bloomSlider.value : 0);
        PlayerPrefs.Save();
    }
    public void SetMotionToggle()
    {
        bool enabled = motionToggle.isOn;

        if (!enabled)
        {
            if (motionSlider.value > 0)
                lastMotionValue = motionSlider.value;

            motionSlider.value = 0;
        }
        else
        {
            motionSlider.value = lastMotionValue;
        }

        motionText.color = new Color(motionText.color.r, motionText.color.g, motionText.color.b, enabled ? 1f : 0.5f);

        GlobalVolumeManager.Instance.EnableMotionBlur(enabled);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetInt(motionEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.SetFloat(motionKey, enabled ? motionSlider.value : 0);
        PlayerPrefs.Save();

    }
    public void SetFilmToggle()
    {
        bool enabled = filmToggle.isOn;

        if (!enabled)
        {
            if (filmSlider.value > 0)
                lastFilmValue = filmSlider.value;

            filmSlider.value = 0;
        }
        else
        {
            filmSlider.value = lastFilmValue;
        }

        filmText.color = new Color(filmText.color.r, filmText.color.g, filmText.color.b, enabled ? 1f : 0.5f);

        GlobalVolumeManager.Instance.EnableFilmGrain(enabled);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetInt(filmEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.SetFloat(filmKey, enabled ? filmSlider.value : 0);
        PlayerPrefs.Save();
    }


    // RESET
    public void ResetValuesToBase()
    {
        bloomSlider.value = baseBloomValue;
        motionSlider.value = baseMotionValue;
        filmSlider.value = baseFilmValue;

        bloomToggle.isOn = true;
        motionToggle.isOn = true;
        filmToggle.isOn = true;

        GlobalVolumeManager.Instance.SetBloomIntensity(baseBloomValue);
        GlobalVolumeManager.Instance.SetMotionBlurIntensity(baseMotionValue);
        GlobalVolumeManager.Instance.SetFilmGrainIntensity(baseFilmValue);

        GlobalVolumeManager.Instance.EnableBloom(true);
        GlobalVolumeManager.Instance.EnableMotionBlur(true);
        GlobalVolumeManager.Instance.EnableFilmGrain(true);

    }


    // RESOLUTION
    private void SetupResolutionDropdown()
    {
        Resolution[] allResolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        filteredResolutions.Clear();

        Dictionary<string, Resolution> bestResolutions = new Dictionary<string, Resolution>();

        foreach (Resolution resolution in allResolutions)
        {
            string key = resolution.width + "x" + resolution.height;

            if (!bestResolutions.ContainsKey(key))
            {
                bestResolutions.Add(key, resolution);
            }
            else
            {
                Resolution existing = bestResolutions[key];

                if (GetRefreshRate(resolution) > GetRefreshRate(existing))
                {
                    bestResolutions[key] = resolution;
                }
            }
        }

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        foreach (Resolution resolution in bestResolutions.Values)
        {
            filteredResolutions.Add(resolution);

            string option = $"{resolution.width} x {resolution.height}";
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = filteredResolutions.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count)
            return;

        Resolution resolution = filteredResolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRateRatio);

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetInt(resolutionKey, resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        if (isLoadingSettings)
            return;

        PlayerPrefs.SetInt(fullScreenKey, isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private double GetRefreshRate(Resolution resolution)
    {
        return resolution.refreshRateRatio.value;
    }
}
