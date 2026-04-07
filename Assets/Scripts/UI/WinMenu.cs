using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public void LoadStartMenu()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        ThisSceneManager.Instance.LoadNewScene("CharacterSelectScreen"); // Load the start menu scene
    }

    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        ThisSceneManager.Instance.LoadNewScene("LobbyMap"); // Load the lobby scene
    }
}