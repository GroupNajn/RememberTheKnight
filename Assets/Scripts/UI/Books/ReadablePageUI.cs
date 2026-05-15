using TMPro;
using UnityEngine;

public class ReadablePageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pageText;

    public void SetText(string text)
    {
        pageText.text = text;
    }
}
