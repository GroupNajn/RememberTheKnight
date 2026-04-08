using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneToLoad;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        if (sceneToLoad != string.Empty)
        {
            ThisSceneManager.Instance.LoadScene(sceneToLoad);
        }
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if (sceneToLoad != string.Empty)
        {
            ThisSceneManager.Instance.LoadScene(sceneToLoad);
        }

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        ThisSceneManager.Instance.ActivateScene(sceneToLoad);
    }
}