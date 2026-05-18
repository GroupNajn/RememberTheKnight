using UnityEngine;

public class RegularFogWall : MonoBehaviour
{
    [SerializeField] int killedEnemiesToTurnOff = 5;
    int killedEnemies = 0;
    GameObject wall;

    private void Awake()
    {
        killedEnemies = 0;
        wall = GetComponentInChildren<ParticleSystem>().gameObject;
        Event_System.instance.OnEnemyKilledNew += OnEnemyKilled;
    }

    private void OnEnemyKilled(EnemyLootProfile profile, Vector3 vector)
    {
        killedEnemies++;
        if (killedEnemies >= killedEnemiesToTurnOff)
        {
            TurnOffWall();
        }
    }

    private void Start()
    {
        TurnOnWall();
    }

    void TurnOnWall()
    {
        wall.SetActive(true);
    }

    void TurnOffWall()
    {
        wall.SetActive(false);
    }

    private void OnDisable()
    {
        Event_System.instance.OnEnemyKilledNew -= OnEnemyKilled;
    }
}