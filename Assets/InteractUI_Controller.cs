using UnityEngine;
using TMPro;
public class InteractUI_Controller : MonoBehaviour
{
    public Canvas Canvas { get => Canvas; private set => Canvas = value; }
    public TextMeshProUGUI Tmp { get => Tmp; private set => Tmp = value; }


    void Start()
    {
        Canvas = GetComponentInChildren<Canvas>();
        Canvas.worldCamera = Camera.main;
        Tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {

    }
}
