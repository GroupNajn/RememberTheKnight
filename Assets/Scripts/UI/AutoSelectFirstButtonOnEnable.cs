using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectFirstButtonOnEnable : MonoBehaviour
{
    [SerializeField] protected GameObject FirstSelectedButton;
    protected virtual void OnEnable()
    {
        //HandleInputChanged();
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

        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        //lastSelected = FirstSelectedButton;
    }

    private void ClearSelection()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

