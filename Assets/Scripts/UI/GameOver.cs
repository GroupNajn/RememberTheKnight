using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void LoadLobby()
    {
        Time.timeScale = 1f; // Ensure the game is not paused when loading the main menu
        ThisSceneManager.Instance.LoadScene("LobbyMap"); // Load the lobby scene
    }
}