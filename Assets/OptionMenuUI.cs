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
        uiManager.CloseOptionMenu();
        uiManager.OpenAudioUI();
    }

    public void OpenVideo()
    {
        uiManager.CloseOptionMenu();
        uiManager.OpenVideoUI();
    }

}
