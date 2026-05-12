using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    public float DebugRay_DrawTime = 5f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    List<GameObject> damageables = new();
    GameObject player;
    GameObject[] hitchecks;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionMaxDamage = 30f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damageables.Add(player);

        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
    }

    public void OnExplode()
    {
        bool exploded = false;
        foreach (var damageable in damageables)
        {
            if (damageable == null || gameObject == null) continue;
            bool damageableHit = false;
            float distanceToDamageable = Vector3.Distance(transform.position, damageable.transform.position);

            for (int i = 0; i < hitchecks.Length; i++)
            {
                Ray ray = new Ray(transform.position, hitchecks[i].transform.position - transform.position);

                if (Physics.Raycast(ray, out RaycastHit hit, explotionRadius, 8))
                {
                    CharacterController cc = hit.collider.GetComponent<CharacterController>();
                    IKnockbackable knockbackeble = hit.collider.GetComponent<PlayerController>();

                    if (cc != null && distanceToDamageable <= explotionRadius)
                    {
                        damageableHit = true;
                        Debug.DrawRay(transform.position, hitchecks[i].transform.position - transform.position, Color.green, DebugRay_DrawTime);
                        if (knockbackeble != null)
                        {
                            knockbackeble.ApplyKnockback(explosionForce, explotionRadius, transform.position);
                        }
                        break;
                    }
                    // Debug.DrawRay(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position, Color.red, DebugRay_DrawTime);
                }
            }
            if (exploded == false)
            {
                exploded = true;
                ParticleSystem explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
            }
            if (damageableHit)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.SetDamageAmount(MathF.Round(Mathf.Lerp(0, explosionMaxDamage, 1 - (distanceToDamageable / explotionRadius)), 0));
                damageable.GetComponent<IDamageable>().TakeDamage(damageInfo, Vector3.zero);
            }
        }

    }

    public void Explode()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        bool playerHit = false;

        for (int i = 0; i < hitchecks.Length; i++)
        {
            Ray ray = new Ray(transform.position, hitchecks[i].transform.position - transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CharacterController cc = hit.collider.GetComponent<CharacterController>();

                if (cc != null || distanceToPlayer <= 1f)
                {
                    playerHit = true;
                    //Debug.DrawRay(transform.position, hitchecks[i].transform.position - transform.position, Color.green, explotionInterval);
                    IKnockbackable damageable = hit.collider.GetComponent<IKnockbackable>();
                    if (damageable != null)
                    {
                        damageable.ApplyKnockback(explosionForce, explotionRadius, transform.position);
                    }
                    break;
                }
                //Debug.DrawRay(transform.position, hitchecks[i].transform.position - transform.position, Color.red, explotionInterval);
            }
        }
        if (playerHit)
        {
            // Debug.Log("Player is hit by the explosion!");
        }
        else
        {
            // Debug.Log("Player is protected from the explosion by an obstacle!");
        }
    }
}