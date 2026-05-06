using UnityEngine;

public class OptionMenuUI : AutoSelectFirstButtonOnEnable
{
    UIManager uiManager;
    //SceneData sceneData;
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }

    public void OpenControlls()
    {
        uiManager.CloseOptionMenu();
        uiManager.OpenControllsUI();
    }

    public void OpenAudio()
    {
        // NÄR VI LÄGGER TILL AUDIO INSTÄLLNINGAR
    }

    public void OpenVideo()
    {
        // NÄR VI LÄGGER TILL VIDEO INSTÄLLNINGAR
    }

    //public void BackToStartMenu()
    //{
    //    uiManager.OpenStartMenu();
    //    uiManager.CloseStartOptionMenu();
    //}
    //public void BackToPauseMenu()
    //{
    //    uiManager.OpenPauseMenu();
    //    uiManager.ClosePauseOptionMenu();
    //}
}
