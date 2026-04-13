using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKeepBetweenScene : MonoBehaviour
{
    public static PlayerKeepBetweenScene Instance { get; private set; }

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Teleport(Transform targetTransform)
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StartCoroutine(MoveToSpawnPoint());
        //transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        //transform.rotation = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation;
    }

    IEnumerator MoveToSpawnPoint()
    {
        yield return null; // Wait for one frame to ensure the scene is fully loaded

        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn");

        while (spawnPoint == null)
        {
            yield return null; // Wait until the PlayerSpawn object is found
        }

        transform.position = spawnPoint.transform.position;
        transform.rotation = spawnPoint.transform.rotation;
    }
}