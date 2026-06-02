using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-04-02
    /// Camera manager and cameras isn't destroyed when switching scene
    /// 
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

    private void Update()
    {
        worldSpaceCamera.transform.position = Camera.main.transform.position;
        worldSpaceCamera.transform.rotation = Camera.main.transform.rotation;
    }
}