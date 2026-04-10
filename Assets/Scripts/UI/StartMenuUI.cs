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

        // LOAD NEXT SCENE
        //SceneManager.LoadScene(sceneData.sceneIndex[1]);
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