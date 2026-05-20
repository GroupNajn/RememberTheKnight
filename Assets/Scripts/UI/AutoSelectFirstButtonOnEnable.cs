using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectFirstButtonOnEnable : MonoBehaviour
{
    [SerializeField] protected GameObject FirstSelectedButton;
    //[SerializeField] protected GameObject lastSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
    }
    protected virtual void Start()
    {
        HandleInputChanged();
        //Event_System.instance.OnDeviceChanged += HandleInputChanged;
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

