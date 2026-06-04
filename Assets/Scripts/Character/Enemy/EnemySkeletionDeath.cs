using System.Collections;
using UnityEngine;

/// <summary>
/// Turns the enemy into a ragdoll when its health reaches zero, then after a timeout turns the ragdoll into a prefab
/// </summary>
/// <remarks>Author: Theo Johansson and Anton Bockman</remarks>
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
        var bonePile = Instantiate(prefab, transform.position, Quaternion.identity);
        bonePile.layer = gameObject.layer;
        Destroy(gameObject);
    }
    [SerializeField] EnemyRagdoll ragdoll;
    [SerializeField] GameObject prefab;
    [SerializeField] float timeOutSeconds = 10f;
}
