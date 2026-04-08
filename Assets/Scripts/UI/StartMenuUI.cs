using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenuUI : MonoBehaviour
{
    GameObject startMenu;
    GameObject characterSwitchUI;
    PlayerInput playerInput;

    void Awake()
    {
        startMenu = transform.Find("StartMenu").gameObject;
        characterSwitchUI = transform.Find("CharacterSwitchUI").gameObject;


    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        playerInput.enabled = false;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
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