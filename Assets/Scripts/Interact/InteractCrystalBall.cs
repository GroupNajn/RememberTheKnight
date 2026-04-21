using System;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    string sceneName;

    public InteractableUIData UIData { get => UIData; private set => UIData = value;}

    private void Start()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        sceneName = SceneData.Instance[sceneToLoadIndex];

        Event_System.instance.OnLoadScenes += OnLoadScenes;
        //GlobalSceneManager.Instance.LoadScene(sceneName);
    }

    public void Interact()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        GlobalSceneManager.Instance.ActivateSceneTransition(sceneName);
    }

    private void OnLoadScenes()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        GlobalSceneManager.Instance.LoadScene(sceneName);

        Event_System.instance.OnLoadScenes -= OnLoadScenes;
    }
}