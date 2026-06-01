using Unity.Behavior;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] BehaviorGraphAgent boss;
    BlackboardVariable<bool> bossFightStarted;
    void Start()
    {
        if (boss.BlackboardReference.GetVariable("Boss Fight Started", out bossFightStarted)) { }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Event_System.instance.OnSpawnBoss?.Invoke();
            gameObject.SetActive(false);
            if (bossFightStarted != null)
            {
                bossFightStarted.Value = true;
            }
        }
    }
}