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
        uiManager.UIMenuActive = false;

        // LOAD NEXT SCENE
        SceneManager.LoadScene(SceneData.Instance[1]);

        uiManager.OpenCharacterSelectUI();

    }

    public void OpenStartOptions()
    {
        uiManager.OpenStartOptionMenu();
        uiManager.CloseStartMenu();
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}