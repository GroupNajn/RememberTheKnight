using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelectFirstButtonOnEnable : MonoBehaviour 
{
    [SerializeField] protected GameObject FirstSelectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
    }
    protected virtual void Start()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
    }
}
