using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    LTDescr currentTween;
    private int tweenID;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(tweenID);

        tweenID = LeanTween.scale(rect, Vector3.one * 1.05f, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(tweenID);

        tweenID = LeanTween.scale(rect, Vector3.one, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
    }
}
