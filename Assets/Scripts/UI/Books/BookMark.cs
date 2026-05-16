using UnityEngine;
using UnityEngine.EventSystems;

public class BookMark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rect;

    [Header("Positions")]
    [SerializeField] private Vector3 normalY;
    [SerializeField] private Vector3 hoverY;
    [SerializeField] private Vector3 selectedY;

    private bool isSelected;

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        
        LeanTween.cancel(rect);

        Vector3 targetPos = isSelected ? selectedY : normalY;
        LeanTween.move(rect, targetPos, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected)
            return;
        
        LeanTween.cancel(rect);
        LeanTween.move(rect, hoverY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
            return;

        LeanTween.cancel(rect);
        LeanTween.move(rect, normalY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }
}
