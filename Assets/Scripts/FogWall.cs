using UnityEngine;

public class FogWall : MonoBehaviour
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