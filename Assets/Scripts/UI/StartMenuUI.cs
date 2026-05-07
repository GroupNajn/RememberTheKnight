using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
    protected override void Start()
    {
        base.Start();
        uiManager = GetComponentInParent<UIManager>();

        Event_System.instance.OnLoadScenes += OnLoadScene;
    }
    public void StartGame()
    {
        // LOAD NEXT SCENE
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[1]);
        canUseInput = false;
    }

    void OnLoadScene()
    {
        // Close menues when the screen is black
        if (SceneManager.GetActiveScene().name == SceneData.Instance[1])
        {
            uiManager.CloseStartMenu();
            //uiManager.CloseBackgroundUI();
            uiManager.UIMenuActive = false;

            uiManager.OpenCharacterSelectUI();

            // TURNS OFF THE SOULS CANVAS WHEN IN CHARCATER SELECT
            uiManager.CloseSoulUI();
            uiManager.CloseCupUI();
        }
    }


    public void OpenOptions()
    {
        if (canUseInput)
        {
            uiManager.OpenOptionMenu();
            uiManager.CloseStartMenu();
        }
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}