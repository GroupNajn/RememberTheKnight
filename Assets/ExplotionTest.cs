using UnityEngine;

public class ExplotionTest : MonoBehaviour
{
    float time = 0;
    public float explotionInterval = 0.1f;
    public float explotionRadius = 5.0f;
    public float explosionForce = 10f;
    GameObject player;
    GameObject[] hitchecks;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hitchecks = GameObject.FindGameObjectsWithTag("HitCheck");
    }

    void Update()
    {
        time += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (time >= explotionInterval && distanceToPlayer <= explotionRadius)
        {
            Debug.Log(distanceToPlayer);
            time = 0;
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
}