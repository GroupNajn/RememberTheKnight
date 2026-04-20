using UnityEngine;

public class SceneData : MonoBehaviour
{
    int staticSceneCount = 2;
    int runSceneCount = 3;

    public string this[int sceneIndex]
    {
        get
        {
            switch(sceneIndex)
            {
                case 0:
                    return "StartScreen";
                case 1:
                    return "CharacterSelectScreen";
                case 2:
                    return "LobbyMap";
                case 3:
                    return "First_Level";
                case 4:
                    return "Bugfixing_Scene";
                case 5:
                    return "RangedTest";
                case 6:
                    return "MainScene";
                case int runSceneIndex when (runSceneIndex > staticSceneCount && runSceneIndex <= staticSceneCount + runSceneCount):
                    return $"Run_{runSceneIndex - staticSceneCount}";
                default:
                    return "LobbyMap";
            }
        }
    }

    public static SceneData Instance { get; private set; }

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
}