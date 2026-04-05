using UnityEngine;

[RequireComponent(typeof(EnemyDamage))]
public class EnemyExplodeAttack : MonoBehaviour
{
    ParticleSystem explosion;
    float time = 0;
    public float explotionInterval = 0.1f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    GameObject player;
    GameObject[] hitchecks;

    EnemyDamage enemyDamage;
    GameObject barrel;
    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>();

        player = GameObject.FindGameObjectWithTag("Player");
        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
        explosion = GetComponentInChildren<ParticleSystem>();
        barrel = FindChildRecursive(transform, "SM_Prop_Barrel_Open_01").gameObject;
    }

    public void OnExplode()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        bool playerHit = false;
        explosion.Play();
        enemyDamage.TakeDamage(enemyDamage.Health, Vector3.zero);
        Destroy(barrel);

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

    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }
}
