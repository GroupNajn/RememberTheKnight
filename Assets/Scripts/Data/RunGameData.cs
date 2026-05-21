using UnityEngine;

public class RunGameData : MonoBehaviour
{
    public static RunGameData Instance { get; private set; }

    [SerializeField] int minLevelsBeforeBoss = 5;
    [SerializeField] int maxLevelsBeforeBoss = 7;

    bool hasStarted = false;
    public int LevelCounter { get; private set; } = 0;
    public int LevelsBeforeBoss { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            Event_System.instance.OnLobbyLoaded += OnLobbyLoaded;
        }
    }

    public void IncrementLevelCounter()
    {
        if (hasStarted)
        {
            LevelCounter++;
            Debug.Log($"Level Counter incremented to {LevelCounter}");
        }
        else
        {
            hasStarted = true;
            LevelsBeforeBoss = Random.Range(minLevelsBeforeBoss, maxLevelsBeforeBoss + 1);
        }
    }

    private void OnLobbyLoaded()
    {
        LevelCounter = 0;
        hasStarted = false;
    }
}