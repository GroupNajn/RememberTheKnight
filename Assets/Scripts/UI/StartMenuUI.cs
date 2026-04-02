using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    GameObject startMenu;
    GameObject characterSwitchUI;

    void Awake()
    {
        startMenu = transform.Find("StartMenu").gameObject;
        characterSwitchUI = transform.Find("CharacterSwitchUI").gameObject;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        characterSwitchUI.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}