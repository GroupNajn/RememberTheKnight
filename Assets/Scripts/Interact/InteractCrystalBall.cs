using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractCrystalBall : MonoBehaviour, IInteractable, IInteractableUI
{
    [SerializeField] int sceneToLoadIndex;
    [SerializeField] bool preLoadScene = false;
    
    private InteractUI_Controller interactUI_Controller;
    private bool canShowUI = false;
    string sceneName;

    private void Start()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        interactUI_Controller = GetComponent<InteractUI_Controller>();

        sceneName = SceneData.Instance[sceneToLoadIndex];

        Event_System.instance.OnLoadScenes += OnLoadScenes;
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

        if (preLoadScene)
        {
            GlobalSceneManager.Instance.LoadScene(sceneName);
        }

        Event_System.instance.OnLoadScenes -= OnLoadScenes;
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        
        if(sceneToLoadIndex < 0)
        {
            UIData.InfoText = string.Empty;
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

    public void ShowUI()
    {
        if (canShowUI && interactUI_Controller)
        { interactUI_Controller.EnableCanvasObject(); }
            //{ return; }

    }

    public void HideUI()
    {
        if (interactUI_Controller)
        {
            interactUI_Controller.DisableCanvas();
        }
    }

    public void SetLookedAt(bool value)
    {
        canShowUI = value;
    }
}