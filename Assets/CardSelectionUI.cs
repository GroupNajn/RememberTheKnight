using UnityEngine;

public class CardSelectionUI : MonoBehaviour
{
    GameObject cardSelectUI;
    void Awake()
    {
        cardSelectUI = transform.Find("CardSelectUI").gameObject;
    }

    public void OpenCardSelectUI()
    {
        cardSelectUI.SetActive(true);
    }
}
