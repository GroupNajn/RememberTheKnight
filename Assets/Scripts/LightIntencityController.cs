using UnityEngine;

public class LightIntencityController : MonoBehaviour
{
    private Light lightToControl;

    [Header("Light Intensity Settings")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float intensityChangeSpeed = 0.5f;

    private float noiseOffset;

    private void Start()
    {
        if (lightToControl == null)
            lightToControl = GetComponentInChildren<Light>();

        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * intensityChangeSpeed, noiseOffset);
        lightToControl.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        float intencityToColor = Mathf.InverseLerp(minIntensity, maxIntensity, lightToControl.intensity);

        Color lowColor = new Color(1f, 0.6f, 0.2f);
        Color highColor = new Color(1f, 0.5f, 0.2f);

        lightToControl.color = Color.Lerp(lowColor, highColor, intencityToColor);
    }
}

