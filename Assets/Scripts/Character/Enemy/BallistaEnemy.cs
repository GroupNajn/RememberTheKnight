using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaEnemy : MonoBehaviour
{

    // Condition for destroying ballista
    [Header("Enemies")]
    [SerializeField] private List<GameObject> requiredEnemies = new List<GameObject>();

    [Header("Ballista Specifics")]
    private HashSet<GameObject> barrels = new HashSet<GameObject>();
    [SerializeField] private GameObject destroyedBallista;

    private void Start()
    {
        Event_System.instance.OnEnemyKilled += OnEnemyKilled;

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.name.Contains("ExplosiveBarrel")) barrels.Add(child.gameObject);
        }

    }

    private void OnEnemyKilled(EnemyDamage damage)
    {

        if (requiredEnemies.Contains(damage.gameObject))
        {
            requiredEnemies.Remove(damage.gameObject);
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
        Destroy(gameObject);
    }
}
