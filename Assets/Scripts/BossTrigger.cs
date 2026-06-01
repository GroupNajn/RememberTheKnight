using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Event_System.instance.OnSpawnBoss?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void Update() // TO BE REMOVED
    {
        if (Input.GetKey(KeyCode.P))
        {
            Event_System.instance.OnBossDeath?.Invoke();
        }
    }
}