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
    [SerializeField] float heightScaler= 1f;
    [SerializeField] float heightDecreaser = 1f;


    private void Awake()
    {
        tmp = GetComponentInParent<TextMeshProUGUI>();
        origin = transform.position;
    }



    void Update() // The  following variables are evalutaed with the respect of time of the AnimationCurves in the inspector.
                  // The scaler, and decreaser variables are to sclae the changes in proportion to the scale of our game units. 
    {  
        tmp.color = new Color(1,1,1, opacityCurve.Evaluate(time));
        transform.localScale = (Vector3.one * (scaleCurve.Evaluate(time) * sizeScaler) / sizeDecreaser);
        transform.position = origin + new Vector3(0, ((heightCurve.Evaluate(time) * heightScaler )/ heightDecreaser), 0);
        time += Time.deltaTime;


    }
}
