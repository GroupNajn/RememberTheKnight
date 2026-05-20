using TMPro;
//using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI text;
    private Color originalColor;
    private Color hoverColor = Color.white;
    public Button button;
    private RectTransform rect;
    LTDescr currentTween;
    private int tweenID;

    //Script made by Henric 2026-05-10


    void Start()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = text.color;
        button = GetComponent<Button>();
        button.onClick.AddListener(ResetValues);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        //LeanTween.scale(rect, Vector3.one, 0f);
        //text.color = originalColor;
        tweenID = -1;
        //button.clicked += ResetValues;
    }



    /* Uses Unity's inbuilt EventSystem when mouse cursor hovers over a button this method is called
     *  It rescales the buttons RectTransform to Vector3(1.05, 1.05, 1.05) over a duration of 0.2 seconds.
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        tweenID = LeanTween.scale(rect, Vector3.one * 1.05f, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
        text.color = hoverColor;
    }
    /* Uses Unity's inbuilt EventSystem when mouse cursor hovers over a button this method is called
     *  It rescales the buttons RectTransform to Vector3.one over a duration of 0.2 seconds.
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);

        tweenID = LeanTween.scale(rect, Vector3.one, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
        text.color = originalColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            LeanTween.cancel(gameObject);

            tweenID = LeanTween.scale(rect, Vector3.one * 1.05f, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
            text.color = hoverColor;
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            LeanTween.cancel(gameObject);

            tweenID = LeanTween.scale(rect, Vector3.one, 0.2f).setEaseOutQuad().setIgnoreTimeScale(true).id;
            text.color = originalColor;
        }
    }

    private void ResetValues()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(rect, Vector3.one, 0f);
        text.color = originalColor;
    }


    private void OnDisable()
    {
        //button.clicked -= ResetValues;
        LeanTween.cancel(gameObject);
        tweenID = -1;
    }

    private void OnDestroy()
    {
        LeanTween.cancel(gameObject);
        tweenID = -1;
        //button.clicked -= ResetValues;
    }


}
