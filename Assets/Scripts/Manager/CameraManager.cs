using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Camera worldSpaceCamera;
    //[SerializeField] private Transform player;
    //[SerializeField] private Vector3 offset;
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

    //private void Update()
    //{
    //    worldSpaceCamera.transform.position = Camera.main.transform.position;
    //    worldSpaceCamera.transform.rotation = Camera.main.transform.rotation;
    //}

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