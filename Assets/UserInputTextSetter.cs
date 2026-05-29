using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public class UserInputTextSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputText;

    [SerializeField] bool bookText;
    [SerializeField] bool cupText;
    [SerializeField] bool holsterText;

    private void Awake()
    {
        if (bookText && (cupText || holsterText))
        {
            cupText = false;
            holsterText = false;
        }
    }
    private void Update() // LOOK IF THIS CAN BE OPTIMIZED
    {
        if (bookText)
        {
            string letter =InputManager.Instance.SetOpenBookBinding();
            inputText.text = ($"[{letter}]");
        }
        else if (cupText)
        {
            string letter = InputManager.Instance.SetHealBinding();
            inputText.text = ($"[{letter}]");
        }
        else if (holsterText)
        {
            string letter = InputManager.Instance.SetHolsterBinding();
            inputText.text = ($"[{letter}]");
        }


    }
}
