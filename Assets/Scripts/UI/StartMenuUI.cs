using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
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
    public void StartGame()
    {
        uiManager.CloseStartMenu();
        uiManager.CloseBackgroundUI();
        uiManager.UIMenuActive = false;

        // LOAD NEXT SCENE
        GlobalSceneManager.Instance.LoadSceneNoTransition(SceneData.Instance[1]);

        uiManager.OpenCharacterSelectUI();

        // TURNS OFF THE SOULS CANVAS WHEN IN CHARCATER SELECT
        uiManager.CloseSoulUI();

    }

    public void OpenOptions()
    {
        uiManager.OpenOptionMenu();
        uiManager.CloseStartMenu();
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}