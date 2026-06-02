using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectFirstButtonOnDisable : MonoBehaviour
{
    [SerializeField] protected GameObject FirstSelectedButton;
    [SerializeField] bool useLastSelectedButton = true;
    GameObject lastSelectedButton;
    public static bool IsGoingBack { get; set; }

    // ITS IMPORTANT THAT THIS IS USED WITH CARE, CAN CAUSE ISSUES IF NOT USED PROPERLY

    protected virtual void OnDisable()
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
        if (FirstSelectedButton == null)
            return;

        if (lastSelectedButton != null && IsGoingBack)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        }

        IsGoingBack = false;
    }

    private void ClearSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

}
