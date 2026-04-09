using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    public static PlayerKeepBetweenScene Instance { get; private set; }
    bool hasMovedToSpawn = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = ThisSceneManager.Instance.PlayerSpawnPosition.position;
        transform.rotation = ThisSceneManager.Instance.PlayerSpawnPosition.rotation;

        //StartCoroutine(SetPlayerPosition());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    IEnumerator SetPlayerPosition()
    {
        yield return null;

        transform.position = ThisSceneManager.Instance.PlayerSpawnPosition.position;
        transform.rotation = ThisSceneManager.Instance.PlayerSpawnPosition.rotation;
    }
}