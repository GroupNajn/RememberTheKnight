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

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when in the start menu
        Cursor.visible = true; // Show the cursor when in the start menu
    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        playerInput.enabled = false;

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when in the start menu
        Cursor.visible = true; // Show the cursor when in the start menu
    }

    //private void Update()
    //{
    //    Cursor.lockState = CursorLockMode.None; // Unlock the cursor when in the start menu
    //    Cursor.visible = true; // Show the cursor when in the start menu
    //}

    public void StartGame()
    {
        playerInput.enabled = false;
        startMenu.SetActive(false);
        characterSwitchUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor when in the start menu
        Cursor.visible = true; // Show the cursor when in the start menu
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}