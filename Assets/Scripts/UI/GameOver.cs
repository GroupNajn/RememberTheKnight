using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : AutoSelectFirstButtonOnEnable
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        Event_System.instance.OnLobbyLoaded += OnLobbyLoaded;
    }
    
    public void LoadLobby()
    {
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]); // Load the lobby scene
    }

    private void OnLobbyLoaded()
    {
        UIManager.Instance.HideActiveUI();
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<TargetLockHandler>().SceneSwitch(); // unlock camera and re center;
    }
}