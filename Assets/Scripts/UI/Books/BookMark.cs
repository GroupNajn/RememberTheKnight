using UnityEngine;
using UnityEngine.EventSystems;

public class BookMark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    /// <summary>
    /// Created by Anton 2026-05-17
    /// Initially created as a component to handle the individual bookmarks in the book UI, 
    /// which are used to switch between different pages in the book.
    /// Also uses animations to move the bookmark up when hovered or selected, and back down when not hovered or selected.
    /// </summary>

    private BookUi bookUi;

    [SerializeField] private RectTransform rect;

    [Header("Positions")]
    [SerializeField] private Vector3 normalY;
    [SerializeField] private Vector3 hoverY;
    [SerializeField] private Vector3 selectedY;

    private bool isSelected;

    private void Start()
    {
        bookUi = FindFirstObjectByType<BookUi>(FindObjectsInactive.Include);
    }

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

        if (bookUi.isAnimating)
            return;

        LeanTween.cancel(rect);
        LeanTween.move(rect, hoverY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
            return;

        if (bookUi.isAnimating)
            return;

        LeanTween.cancel(rect);
        LeanTween.move(rect, normalY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isSelected)
            return;

        if (bookUi.isAnimating)
            return;

        LeanTween.cancel(rect);
        LeanTween.move(rect, hoverY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (isSelected)
            return;

        if (bookUi.isAnimating)
            return;

        LeanTween.cancel(rect);
        LeanTween.move(rect, normalY, 0.3f).setEaseOutQuad().setIgnoreTimeScale(true);
    }
}
