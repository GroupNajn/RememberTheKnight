using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectFirstButtonOnEnable : MonoBehaviour
{
    [SerializeField] protected GameObject FirstSelectedButton;
    [SerializeField] bool useLastSelectedButton = true;
    GameObject lastSelectedButton;
    public static bool IsGoingBack { get; set; }

    protected virtual void OnEnable()
    {
        HandleInputChanged();
    }

    protected virtual void Awake()
    {
        HandleInputChanged();
        Event_System.instance.OnDeviceChanged += HandleInputChanged;
    }

    public void HandleInputChanged()
    {
        if (InputManager.Instance.usingGamepad)
        {
            SelectButton();
        }
        else
        {
            ClearSelection();
        }
    }

    private void SelectButton()
    {
        Debug.Log($"Selecting button in {name}");

        if (FirstSelectedButton == null || !gameObject.activeSelf)
            return;

        if (lastSelectedButton != null && IsGoingBack)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        }

        Debug.Log($"Selected gameobject set to {EventSystem.current.currentSelectedGameObject.name} in {name}");

        IsGoingBack = false;
    }

    private void ClearSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        if (EventSystem.current != null)
        {
            lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }
}