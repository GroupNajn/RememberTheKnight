using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{

    /// <summary>
    /// Changed by Anton 2026-05-14
    /// Added camera for world space UI, which will follow the main camera and be used to render the world space UI elements.
    /// </summary>
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Camera worldSpaceCamera;
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

    private void Start()
    {
        FindThings();
    }

    private void Update()
    {
        worldSpaceCamera.transform.position = Camera.main.transform.position;
        worldSpaceCamera.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindThings();
    }

    void FindThings()
    {

    }
}