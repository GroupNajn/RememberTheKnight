using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    private Color originalColor;
    private Color hoverColor = Color.white;

    private RectTransform rect;
    LTDescr currentTween;
    private int tweenID;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = text.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        tweenID = -1;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        tweenID = LeanTween.scale(rect, Vector3.one * 1.05f, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
        text.color = hoverColor;
    }

    private void OnDisable()
    {
        
        LeanTween.cancel(gameObject);
        tweenID = -1;
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
        tweenID = -1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        tweenID = LeanTween.scale(rect, Vector3.one, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
        text.color = originalColor;
    }
}
