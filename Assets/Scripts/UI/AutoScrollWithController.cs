using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScrollWithController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private float padding = 30;
    [SerializeField] private float scrollSpeed = 10;

    private RectTransform selectedRect;

    private void Reset()
    {
        scrollRect = GetComponent<ScrollRect>();

        if (scrollRect != null)
        {
            viewport = scrollRect.viewport;
            content = scrollRect.content;
        }

    }

    public void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
            return;

        if (!selected.transform.IsChildOf(content))
            return;

        selectedRect = selected.GetComponent<RectTransform>();

        if (selectedRect == null)
            return;

        if (InputManager.Instance.usingGamepad)
        {
            Scroll();
        }
    }

    public void Scroll()
    {


        // USED TO KNOW WHERE CORNERS ARE AND IF SELECTED ARE ABOVE THAT
        Vector3[] selectedCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];

        selectedRect.GetWorldCorners(selectedCorners);
        viewport.GetWorldCorners(viewportCorners);

        float selectedTop = selectedCorners[1].y;
        float selectedBottom = selectedCorners[0].y;

        float viewportTop = viewportCorners[1].y - padding;
        float viewportBottom = viewportCorners[0].y + padding;

        float delta = 0f;

        if (selectedTop > viewportTop)
        {
            delta = selectedTop - viewportTop;
        }
        else if (selectedBottom < viewportBottom)
        {
            delta = selectedBottom - viewportBottom;
        }

        if (Mathf.Abs(delta) > 0.05f)
        {
            Vector2 position = content.anchoredPosition;
            position.y -= delta;
            content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, position, Time.unscaledDeltaTime * scrollSpeed);
        }

    }
}
