using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    PlayerInput playerInput;
    UIManager uiManager;

    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();
    }
    public void Resume()
    {
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor when resuming
        Cursor.visible = false; // Hide the cursor when resuming
        playerInput.enabled = true; // Enable player input when resuming

        uiManager.ClosePauseMenu(); // Hide the pause menu
    }

    public void OpenPauseOptions()
    {

        uiManager.OpenPauseOptionMenu();
        uiManager.CloseStartMenu();
    }

    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        playerInput.enabled = true; // Enable player input when loading the main menu

        ThisSceneManager.Instance.LoadScene("LobbyMap"); // Load the lobby scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}