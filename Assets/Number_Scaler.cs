using TMPro;

using UnityEngine;

public class Number_Scaler : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private float time = 0f;
    private TextMeshProUGUI tmp;
    private Vector3 origin;

    private void Awake()
    {
        tmp = GetComponentInParent<TextMeshProUGUI>();
        origin = transform.position;
    }



    void Update()
    {
        tmp.color = new Color(1,1,1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;


    }
}
