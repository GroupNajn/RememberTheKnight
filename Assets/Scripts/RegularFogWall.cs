using UnityEngine;
using TMPro;

public class RegularFogWall : MonoBehaviour
{
    [SerializeField] int killedEnemiesToTurnOff = 5;
    [SerializeField] bool onAtStart = true;
    int killedEnemies = 0;
    [SerializeField] GameObject wall;
    [SerializeField] TMP_Text requiermentText;
    [SerializeField] GameObject skull;
    GameObject player;

    private void Awake()
    {
        killedEnemies = 0;
        Event_System.instance.OnEnemyKilledNew += OnEnemyKilled;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (onAtStart)
        {
            TurnOnWall();
        }
        else
        {
            TurnOffWall();
            Event_System.instance.OnEnemyKilledNew -= OnEnemyKilled;
            Event_System.instance.OnLevelCompleted?.Invoke();
        }
    }

    private void OnEnemyKilled(EnemyLootProfile profile, Vector3 vector)
    {
        killedEnemies++;
        requiermentText.text = $"{killedEnemiesToTurnOff - killedEnemies} remaining";
        if (killedEnemies >= killedEnemiesToTurnOff)
        {
            TurnOffWall();
            Event_System.instance.OnEnemyKilledNew -= OnEnemyKilled;
            Event_System.instance.OnLevelCompleted?.Invoke();
        }
    }

    void TurnOnWall()
    {
        wall.SetActive(true);
        requiermentText.text = $"{killedEnemiesToTurnOff - killedEnemies} remaining";
    }

    void TurnOffWall()
    {
        wall.SetActive(false);
        requiermentText.text = "";
        skull.SetActive(false);
    }

    private void OnDisable()
    {
        Event_System.instance.OnEnemyKilledNew -= OnEnemyKilled;
    }
}