using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    float lastBloomValue = 0.25f;
    float lastMotionValue = 0.25f;
    float lastFilmValue = 0.25f;

    float baseBloomValue;
    float baseMotionValue;
    float baseFilmValue;

    bool sliderInput = false;
    protected override void Start()
    {
        uiManager = GetComponentInParent<UIManager>();

        bloomSlider.value = GlobalVolumeManager.Instance.GetBloomIntensity();
        motionSlider.value = GlobalVolumeManager.Instance.GetMotionBlurIntensity();
        filmSlider.value = GlobalVolumeManager.Instance.GetFilmGrainIntensity();

        baseBloomValue = bloomSlider.value;
        baseMotionValue = motionSlider.value;
        baseFilmValue = filmSlider.value;

        base.Start();
    }

    public void SetBloomSlider(float value)
    {
        bloomText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        bloomToggle.isOn = enabled;

        //bloomText.color = new Color(bloomText.color.r, bloomText.color.g, bloomText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetBloomIntensity(value);
    }

    public void SetMotionSlider(float value)
    {
        motionText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        motionToggle.isOn = enabled;

        //motionText.color = new Color(motionText.color.r, motionText.color.g, motionText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetMotionBlurIntensity(value);
    }

    public void SetFilmSlider(float value)
    {
        filmText.text = $"{(int)(value * 100)}";

        bool enabled = value > 0;
        filmToggle.isOn = enabled;

        //filmText.color = new Color(filmText.color.r, filmText.color.g, filmText.color.b, enabled ? 1f : 0.5f);
        GlobalVolumeManager.Instance.SetFilmGrainIntensity(value);
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
    }

    public void ResetValuesToBase()
    {
        bloomSlider.value = baseBloomValue;
        motionSlider.value = baseMotionValue;
        motionSlider.value = baseFilmValue;

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
}
