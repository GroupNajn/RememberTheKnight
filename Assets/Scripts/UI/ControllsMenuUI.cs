using UnityEngine;

public class ControllsMenuUI : MonoBehaviour
{
    UIManager uiManager;
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    //public void BackToStartOptionMenu()
    //{
    //    uiManager.OpenStartOptionMenu();
    //    uiManager.CloseStartControllsUI();
    //}
    //public void BackToPauseOptionMenu()
    //{
    //    uiManager.OpenPauseOptionMenu();
    //    uiManager.ClosePauseControllsUI();
    //}
}
