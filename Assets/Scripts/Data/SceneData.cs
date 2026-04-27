using UnityEngine;

public class SceneData : MonoBehaviour
{
    int staticSceneCount = 100;
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
                    return "Shop_Level";
                case 5:
                    return "Healing_Level";
                case 6:
                    return "CupBoss_Level";
                case 7:
                    return "Second_Level";
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