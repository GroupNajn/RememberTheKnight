using UnityEngine;

public class RegularFogWall : MonoBehaviour
{
    GameObject wall;

    private void Awake()
    {
        wall = GetComponentInChildren<ParticleSystem>().gameObject;
        Event_System.instance.OnLevelCompleted += TurnOffWall;
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

    private void OnDestroy()
    {
        Event_System.instance.OnLevelCompleted -= TurnOffWall;
    }
}