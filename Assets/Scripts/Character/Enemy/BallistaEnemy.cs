using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{
    // Condition for destroying ballista
    [Header("Enemies")]
    [SerializeField] private List<GameObject> requiredEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> requiredSouls = new List<GameObject>();
    [SerializeField] private Vector3 soulOffset;

    [Header("Ballista Specifics")]
    private HashSet<GameObject> barrels = new HashSet<GameObject>();
    [SerializeField] private GameObject destroyedBallista;
    [SerializeField] private GameObject ballistaSoul;
    [SerializeField] private bool IsDestroyed = false;

    void Start()
    {
        Event_System.instance.OnEnemyKilledNew += OnEnemyKilled;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name.Contains("ExplosiveBarrel")) barrels.Add(child.gameObject);
        }

        foreach (GameObject e in requiredEnemies)
        {
            GameObject soul = Instantiate(ballistaSoul, e.transform.position + soulOffset, Quaternion.identity, e.transform);
            requiredSouls.Add(soul);
        }
    }

    void OnEnemyKilled(EnemyLootProfile enemy, Vector3 spawnPos) 
    {
        if (IsDestroyed) return;

        if (requiredEnemies.Contains(enemy.gameObject))
        {
            requiredEnemies.Remove(enemy.gameObject);
            foreach (Transform child in enemy.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Contains("Ballista_Soul")) Destroy(child.gameObject);
            }
        }

        if (requiredEnemies.Count == 0)
        {
            StartCoroutine(ExplodeBarrels());
        }
    }

    IEnumerator ExplodeBarrels()
    {
        foreach (GameObject barrel in barrels)
        {
            barrel.GetComponent<ExplodingBarrel>().Explode();
            yield return new WaitForSeconds(0.1f);
        }

        Instantiate(destroyedBallista, transform.position, Quaternion.identity);
        IsDestroyed = true;
        Destroy(gameObject);
    }
}
