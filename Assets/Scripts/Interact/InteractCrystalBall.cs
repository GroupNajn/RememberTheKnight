using System;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    public bool IsLookedAt { get; set; } = false;

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

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        
        if(sceneToLoadIndex < 0)
        {
            UIData = null;
        }
        else if (sceneToLoadIndex == 2)
        {
            UIData.InfoText = "Touch the crystal ball to return return to lobby.";
        }
        else if (sceneToLoadIndex == 3)
        {
            UIData.InfoText = "Touch the crystal ball to advance to the next stage.";
        }
        else
        {
            UIData.InfoText = "Touch the crystal ball to advance to the next stage.";
        }
        return UIData;
    }
}