using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
public class EnemyExplodeAttack : MonoBehaviour
{
    public float DebugRay_DrawTime = 5f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    GameObject player;
    GameObject[] hitchecks;

    EnemyDamage enemyDamage;
    [SerializeField] GameObject barrel;
    [SerializeField] ParticleSystem explosion;
    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();

        player = GameObject.FindGameObjectWithTag("Player");
        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
        GetComponentsInChildren<Transform>().ToList().ForEach(transform =>
        {
            if (transform.name == "SM_Prop_Barrel_01") barrel = transform.gameObject;
            if (transform.name == "SM_Prop_Barrel_Open_01") barrel = transform.gameObject;
        });
    }

    public void OnExplode()
    {
        float distanceToPlayer = Vector3.Distance(barrel.transform.position, player.transform.position);

        bool playerHit = false;
        //explosion.Play();
        
        for (int i = 0; i < hitchecks.Length; i++)
        {
            Ray ray = new Ray(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit,explotionRadius,8))
            {
                CharacterController cc = hit.collider.GetComponent<CharacterController>();
                IKnockbackable knockbackeble = hit.collider.GetComponent<PlayerController>();

                if (cc != null && distanceToPlayer <= explotionRadius)
                {
                    playerHit = true;
                    Debug.DrawRay(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position, Color.green, DebugRay_DrawTime);
                    if (knockbackeble != null)
                    {
                        knockbackeble.ApplyKnockback(explosionForce, explotionRadius, transform.position);
                    }
                    if(knockbackeble == null) Debug.Log("Explotion: could not find knockbackeble");
                    break;
                }
                Debug.DrawRay(barrel.transform.position, hitchecks[i].transform.position - barrel.transform.position, Color.red, DebugRay_DrawTime);
            }
        }
        if (barrel != null)
        {
            Destroy(barrel);
            var boom = Instantiate(explosion, barrel.transform.position, Quaternion.identity);
            boom.Play();
            enemyDamage.TakeDamage(enemyDamage.Health, Vector3.zero);
        }
        if (playerHit)
        {
            float damage = Mathf.Lerp(0, 30, 1 - (distanceToPlayer / explotionRadius));
            player.GetComponent<PlayerStats>().TakeDamage(damage, Vector3.zero);

        }
    }
}