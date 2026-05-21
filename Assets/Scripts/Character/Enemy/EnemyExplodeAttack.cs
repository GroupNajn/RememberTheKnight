using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(EnemyDamage))]

[RequireComponent(typeof(Animator))]
public class EnemyExplodeAttack : MonoBehaviour
{
    private static readonly int BoomTimeHash = Animator.StringToHash("BoomTime");
    public float DebugRay_DrawTime = 5f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    GameObject player;
    List<GameObject> damageables = new();
    GameObject[] hitchecks;

    EnemyDamage enemyDamage;
    Animator animator;
    [SerializeField] GameObject barrel;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float explosionMaxDamage = 30f;
    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();
        animator = GetComponent<Animator>();

        damageables.Add(GameObject.FindGameObjectWithTag("Player"));
        damageables.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
        GetComponentsInChildren<Transform>().ToList().ForEach(transform =>
        {
            if (transform.name == "SM_Prop_Barrel_01") barrel = transform.gameObject;
            if (transform.name == "SM_Prop_Barrel_Open_01") barrel = transform.gameObject;
        });
    }

    void Update()
    {
        if (enemyDamage.Health / enemyDamage.MaxHealth < 0.5f && !animator.GetBool(BoomTimeHash))
        {
            animator.SetBool(BoomTimeHash, true);
        }
    }

    public void OnExplode()
    {
        bool exploded = false;
        foreach (var damageable in damageables)
        {
            if (damageable == null || barrel == null) continue;
            bool damageableHit = false;
            float distanceToDamageable = Vector3.Distance(barrel.transform.position, damageable.transform.position);

            for (int i = 0; i < hitchecks.Length; i++)
            {
                Ray ray = new Ray(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position);

                if (Physics.Raycast(ray, out RaycastHit hit, explotionRadius, 8))
                {
                    CharacterController cc = hit.collider.GetComponent<CharacterController>();
                    IKnockbackable knockbackeble = hit.collider.GetComponent<PlayerController>();

                    if (cc != null && distanceToDamageable <= explotionRadius)
                    {
                        damageableHit = true;
                        Debug.DrawRay(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position, Color.green, DebugRay_DrawTime);
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
                Destroy(barrel);
                ParticleSystem explosion = Instantiate(this.explosion, barrel.transform.position, Quaternion.identity);
                // explosion.Play();
                DamageInfo damageInfo = new DamageInfo((float)enemyDamage.Health);
                enemyDamage.TakeDamage(damageInfo, Vector3.zero);
            }
            if (damageableHit)
            {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.SetDamageAmount(MathF.Round(Mathf.Lerp(0, explosionMaxDamage, 1 - (distanceToDamageable / explotionRadius)), 0));
                damageable.GetComponent<IDamageable>().TakeDamage(damageInfo, Vector3.zero);
            }
        }

    }
}