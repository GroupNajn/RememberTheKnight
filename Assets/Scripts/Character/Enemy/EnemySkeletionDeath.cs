using System.Collections;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(EnemyRagdoll))]
public class EnemySkeletonDeath : MonoBehaviour, ITriggerable
{
    void Start()
    {
        ragdoll = GetComponent<EnemyRagdoll>();
    }
    public void Trigger()
    {
        if (ragdoll != null) ragdoll.EnableRagdoll();
        StartCoroutine(CrumbleAfter(timeOutSeconds));
    }

    IEnumerator CrumbleAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    [SerializeField] EnemyRagdoll ragdoll;
    [SerializeField] GameObject prefab;
    [SerializeField] float timeOutSeconds = 10f;
}
