using UnityEngine;

public class OptionMenuUI : MonoBehaviour
{
    UIManager uiManager;
    //SceneData sceneData;
    void Awake()
    {
    }
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    public void OpenStartControlls()
    {
        uiManager.CloseStartOptionMenu();
        uiManager.OpenStartControllsUI();
    }
    public void OpenPauseControlls()
    {
        uiManager.ClosePauseOptionMenu();
        uiManager.OpenPauseControllsUI();
    }

    public void OpenAudio()
    {
        // NÄR VI LÄGGER TILL AUDIO INSTÄLLNINGAR
    }

    public void OpenVideo()
    {
        // NÄR VI LÄGGER TILL VIDEO INSTÄLLNINGAR
    }

    public void BackToStartMenu()
    {
        uiManager.OpenStartMenu();
        uiManager.CloseStartOptionMenu();
    }
    public void BackToPauseMenu()
    {
        uiManager.OpenPauseMenu();
        uiManager.ClosePauseOptionMenu();
    }
}
