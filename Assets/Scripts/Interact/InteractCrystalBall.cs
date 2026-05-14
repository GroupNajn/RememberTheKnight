using FMODUnity;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable, IInteractableUIText
{
    [SerializeField] int sceneToLoadIndex;
    [SerializeField] bool preLoadScene = false;

    private bool canShowUI = false;
    string sceneName;

    private void Start()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        sceneName = SceneData.Instance[sceneToLoadIndex];

        Event_System.instance.OnLoadScenes += OnLoadScenes;
    }

    public void Interact()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        RuntimeManager.PlayOneShotAttached (WorldSoundFXManager.instance.teleportEvent,GameObject.FindGameObjectWithTag("Player")); // teleport sound effect

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

        UIData.InfoText = "[F]: Touch orb";

        return UIData;
    }
}