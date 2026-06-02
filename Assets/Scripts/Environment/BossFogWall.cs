using UnityEngine;

public class BossFogWall : MonoBehaviour
{
    GameObject wall;

    private void Awake()
    {
        wall = GetComponentInChildren<ParticleSystem>().gameObject;
        Event_System.instance.OnSpawnBoss += TurnOnWall;       
        Event_System.instance.OnBossDeath += TurnOffWall;       
    }

    private void Start()
    {
        TurnOffWall();
    } 

    void TurnOnWall()
    {
        wall.SetActive(true);
        //bossHealthbar.SetActive(true);
    }

    void TurnOffWall()
    {
        wall.SetActive(false);
    }

    private void OnDestroy()
    {
        Event_System.instance.OnSpawnBoss -= TurnOnWall;
        Event_System.instance.OnBossDeath -= TurnOffWall;
    }
}