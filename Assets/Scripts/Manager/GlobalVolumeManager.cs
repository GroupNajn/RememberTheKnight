using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.Collections.AllocatorManager;

public class GlobalVolumeManager : MonoBehaviour
{
    [SerializeField] Volume globalVolume;
    private VolumeProfile volumeProfile;

    private Bloom Bloom;
    private Vignette Vignette;
    private MotionBlur MotionBlur;
    private ScreenSpaceLensFlare LensFlare;
    private FilmGrain FilmGrain;

    void Awake()
    {
        volumeProfile = globalVolume.profile;

        volumeProfile.TryGet(out Bloom);
        volumeProfile.TryGet(out Vignette);
        volumeProfile.TryGet(out MotionBlur);
        volumeProfile.TryGet(out LensFlare);
        volumeProfile.TryGet(out FilmGrain);
    }

    // BLOOM

    public void EnableBloom(bool enabled)
    {
        Bloom.active = enabled;
    }
    public float GetBloomIntensity()
    {
        return Bloom.intensity.value;
    }
    public void SetBloomThreshold(float value)
    {
        Bloom.threshold.value = value;
    }
    public void SetBloomIntensity(float value)
    {
        Bloom.intensity.value = value;
    }

    public void SetBloomScatter(float value)
    {
        Bloom.scatter.value = value;
    }

    // VIGNETTE

    public void EnableVignette(bool enabled)
    {
        Vignette.active = enabled;
    }

    public void SetVignetteColor(Color color)
    {
        Vignette.color.value = color;
    }

    public void SetVignetteIntensity(float value)
    {
        Vignette.intensity.value = value;
    }

    public void SetVignetteSmoothness(float value)
    {
        Vignette.smoothness.value = value;
    }

    // MOTION BLUR

    public void EnableMotionBlur(bool enabled)
    {
        MotionBlur.active = enabled;
    }
    public float GetMotionBlurIntensity()
    {
        return MotionBlur.intensity.value;
    }

    public void SetMotionBlurIntensity(float value)
    {
        MotionBlur.intensity.value = value;
    }

    public void SetMotionBlurClamp(float value)
    {
        MotionBlur.clamp.value = value;
    }

    // LENS FLARE

    public void EnableLensFlare(bool enabled)
    {
        LensFlare.active = enabled;
    }

    public void SetLensFlareColor(Color color)
    {
        LensFlare.tintColor.value = color;
    }

    public void SetLensFlareIntensity(float value)
    {
        LensFlare.intensity.value = value;
    }

    public void SetLensFlareChromaticAberration(float value)
    {
        LensFlare.chromaticAbberationIntensity.value = value;
    }

    // FILM GRAIN

    public void EnableFilmGrain(bool enabled)
    {
        FilmGrain.active = enabled;
    }

    public void SetFilmGrainIntensity(float value)
    {
        FilmGrain.intensity.value = value;
    }

    public void SetFilmGrainResponse(float value)
    {
        FilmGrain.response.value = value;
    }
}
