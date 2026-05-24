using FMODUnity;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable, IInteractableUIText
{
    [SerializeField] int sceneToLoadIndex;
    [SerializeField] bool preLoadScene = false;

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
        if (sceneToLoadIndex < 0 || GlobalSceneManager.Instance.isTransitioning)
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract == null ||
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract.signed == CardContract.Signed.Not)
        {
            // Add dialouge here
            return;
        }

        RuntimeManager.PlayOneShotAttached (WorldSoundFXManager.instance.teleportEvent,GameObject.FindGameObjectWithTag("Player")); // teleport sound effect

        GlobalSceneManager.Instance.ActivateSceneTransition(sceneName);

        RunGameData.Instance.IncrementLevelCounter();
    }

    private void OnLoadScenes()
    {
        if (sceneToLoadIndex < 0)
        {
            return;
        }

        if (preLoadScene)
        {
            if (RunGameData.Instance.LevelCounter == RunGameData.Instance.LevelsBeforeBoss)
            {
                GlobalSceneManager.Instance.LoadScene(SceneData.Instance[7]);
            }
            else
            {
                GlobalSceneManager.Instance.LoadScene(sceneName);
            }
        }

        Event_System.instance.OnLoadScenes -= OnLoadScenes;
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
         
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract == null ||
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollection>().playerContract.signed == CardContract.Signed.Not)
        {
            UIData.CanInteract = false;
            UIData.InfoText = "Select a family";
        }
        else
        {
            UIData.CanInteract = true;
            UIData.InfoText = "Touch orb";
        }

        return UIData;
    }
}