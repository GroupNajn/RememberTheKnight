public class OptionMenuUI : AutoSelectFirstButtonOnEnable
{
    UIManager uiManager;

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