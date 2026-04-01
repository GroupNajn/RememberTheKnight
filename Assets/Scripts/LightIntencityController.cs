using UnityEngine;

public class LightIntencityController : MonoBehaviour
{
    private Light lightToControl;

    [Header("Light Intensity Settings")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float intensityChangeSpeed = 0.5f;

    Color lowColor;
    Color highColor;

    [SerializeField] Color warmLowColor = new Color(1f, 0.6f, 0.2f);
    [SerializeField] Color warmHighColor = new Color(1f, 0.5f, 0.2f);

    [SerializeField] Color coldLowColor = new Color(1f, 1f, 1f);
    [SerializeField] Color coldHighColor = new Color(1f, 1f, 1f);

    [SerializeField] private bool warmColor = true;

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

        if (warmColor)
        {
            lowColor = warmLowColor;
            highColor = warmHighColor;
        }
        else
        {
            lowColor = coldLowColor;
            highColor = coldHighColor;
        }


        lightToControl.color = Color.Lerp(lowColor, highColor, intencityToColor);
    }
}

