using TMPro;

using UnityEngine;

// Script made by Henric 2026-03-30

public class Number_Scaler : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private float time = 0f;
    private TextMeshProUGUI tmp;
    private Vector3 origin;
    [SerializeField] float sizeScaler = 1f;
    [SerializeField] float sizeDecreaser = 1f;
    [SerializeField] float heightScaler = 1f;
    [SerializeField] float heightDecreaser = 1f;
    [SerializeField] float critScaler = 3f;

    [SerializeField] private Color sneakColor;
    [SerializeField] private Color critColor;
    [SerializeField] private Color normalDamageColor;
    private bool critSwitch = false;
    private bool sneakSwitch = false;


    private void Awake()
    {
        tmp = GetComponentInParent<TextMeshProUGUI>();
        origin = transform.position;
    }

    private void OnDestroy()
    {
        this.sizeScaler = 1f;
        this.sizeDecreaser = 30f;
        this.heightScaler = 1;
        this.heightDecreaser = 1;
    }



    void Update() // The  following variables are evalutaed with the respect of time of the AnimationCurves in the inspector.
                  // The scaler, and decreaser variables are to sclae the changes in proportion to the scale of our game units. 
    {
        UpdateColorOverTime();
        if (critSwitch)
            transform.localScale = (Vector3.one * (scaleCurve.Evaluate(time) * critScaler) / sizeDecreaser);

        transform.localScale = (Vector3.one * (scaleCurve.Evaluate(time) * sizeScaler) / sizeDecreaser);
        transform.position = origin + new Vector3(0, ((heightCurve.Evaluate(time) * heightScaler) / heightDecreaser), 0);
        time += Time.deltaTime;


    }

    public void UpdateColorOverTime()
    {
        if (!sneakSwitch)
        {
            if (critSwitch) tmp.color = new Color(critColor.r, critColor.g, critColor.b, opacityCurve.Evaluate(time));
            else tmp.color = new Color(normalDamageColor.r, normalDamageColor.g, normalDamageColor.b, opacityCurve.Evaluate(time));
        }
        else tmp.color = new Color(sneakColor.r, sneakColor.g, sneakColor.b, opacityCurve.Evaluate(time));

    }
    public void SetCritBoolean(bool isCrit) => critSwitch = isCrit;
    public void SetSneakBoolean(bool isSneak) => sneakSwitch = isSneak;

}
