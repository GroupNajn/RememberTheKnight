using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public void LoadStartMenu()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        GlobalSceneManager.Instance.LoadScene("CharacterSelectScreen"); // Load the start menu scene
    }

    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        GlobalSceneManager.Instance.LoadScene("LobbyMap"); // Load the lobby scene
    }
}