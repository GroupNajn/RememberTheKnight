using System;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    string sceneName;

    private void Start()
    {
        sceneName = SceneData.Instance[sceneToLoadIndex];

        Event_System.instance.OnLoadScenes += OnLoadScenes;
        //GlobalSceneManager.Instance.LoadScene(sceneName);
    }

    public void Interact()
    {
        GlobalSceneManager.Instance.ActivateSceneTransition(sceneName);
    }

    private void OnLoadScenes()
    {
        GlobalSceneManager.Instance.LoadScene(sceneName);
    }
}