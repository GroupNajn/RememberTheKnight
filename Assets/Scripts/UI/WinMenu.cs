using UnityEngine;

public class WinMenu : AutoSelectFirstButtonOnEnable
{
    protected override void Awake()
    {
        base.Awake();

    }

    public void LoadLobby()
    {
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]); // Load the lobby
        Event_System.instance.OnLoadScenes += LeaveBossLevel;
    }

    public void ContinuePlaying()
    {
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[5]); // Load first level
        Event_System.instance.OnLoadScenes += LeaveBossLevel;
    }

    void LeaveBossLevel()
    {
        gameObject.SetActive(false);
        UIManager.Instance.UIMenuActive = false;
        UIManager.Instance.CheckUIState();
        UIManager.Instance.SetTimeScale(true);

        Event_System.instance.OnLoadScenes -= LeaveBossLevel;
    }
}