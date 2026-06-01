using UnityEngine;

public class StartMenuUI : AutoSelectFirstButtonOnEnable
{
    UIManager uiManager;
    private GameData gameData;
    //SceneData sceneData;
    bool canUseInput = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        canUseInput = true;

    }
    protected override void Awake()
    {
        base.Awake();
    }

    public void Start()
    {

        uiManager = GetComponentInParent<UIManager>();
        gameData = FindFirstObjectByType<GameData>();

    }

    public void StartGame()
    {
        canUseInput = false;

        // LOAD NEXT SCENE
        if (!PlayerPrefsSaveSystem.HasPlayedGame())
        {
            GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[1]);
        }
        else
        {
            GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]);
            uiManager.CloseStartMenu();
        }

    }


    public void OpenOptions()
    {
        if (canUseInput)
        {
            uiManager.CloseStartMenu();
            uiManager.OpenOptionMenu();
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}