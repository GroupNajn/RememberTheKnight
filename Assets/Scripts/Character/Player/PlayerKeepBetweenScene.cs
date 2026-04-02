using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    public static PlayerKeepBetweenScene Instance { get; private set; }
    int scenesLoaded = 0;
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
        Debug.Log("PlayerKeepBetweenScene: OnSceneLoaded called for scene " + scene.name);
        scenesLoaded++;
        Debug.Log("PlayerKeepBetweenScene: Total scenes loaded: " + scenesLoaded);
        transform.position = ThisSceneManager.Instance.PlayerSpawnPosition.position;
        transform.rotation = ThisSceneManager.Instance.PlayerSpawnPosition.rotation;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}