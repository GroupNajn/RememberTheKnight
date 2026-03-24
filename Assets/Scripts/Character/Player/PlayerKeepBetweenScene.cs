using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    public static PlayerKeepBetweenScene Instance { get; private set; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = ThisSceneManager.Instance.PlayerSpawnPosition.position;
        transform.rotation = ThisSceneManager.Instance.PlayerSpawnPosition.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ThisSceneManager.Instance.LoadNewScene("SceneManageMent");
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}