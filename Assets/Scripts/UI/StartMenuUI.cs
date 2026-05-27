using UnityEngine;

public class StartMenuUI : AutoSelectFirstButtonOnEnable
{
    UIManager uiManager;
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

    }

    public void StartGame()
    {
        // LOAD NEXT SCENE
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[1]);
        canUseInput = false;
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