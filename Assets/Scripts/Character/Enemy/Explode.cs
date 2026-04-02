using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
public class Explode : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    float time = 0;
    public float explotionInterval = 0.1f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    GameObject player;
    GameObject[] hitchecks;

    EnemyDamage enemyDamage;
    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();

        player = GameObject.FindGameObjectWithTag("Player");
        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
    }

    public void OnExplode()
    {
        enemyDamage.TakeDamage(enemyDamage.Health, Vector3.zero);
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        bool playerHit = false;
        Instantiate(explosion);
        explosion.GetComponentInChildren<ParticleSystem>().Play();
        for (int i = 0; i < hitchecks.Length; i++)
        {
            Ray ray = new Ray(transform.position, hitchecks[i].transform.position - transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                CharacterController cc = hit.collider.GetComponent<CharacterController>();

                if (cc != null || distanceToPlayer <= 1f)
                {
                    playerHit = true;
                    Debug.DrawRay(transform.position, hitchecks[i].transform.position - transform.position, Color.green, explotionInterval);
                    IKnockbackable damageable = hit.collider.GetComponent<IKnockbackable>();
                    if (damageable != null)
                    {
                        damageable.ApplyKnockback(explosionForce, explotionRadius, transform.position);
                    }
                    break;
                }
                Debug.DrawRay(transform.position, hitchecks[i].transform.position - transform.position, Color.red, explotionInterval);
            }
        }
        if (playerHit) player.GetComponent<PlayerStats>().TakeDamage(30, Vector3.zero);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
