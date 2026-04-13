using System.Collections;
using UnityEngine;

public class Projecile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool collided;

    public float speed;
    public Vector3 direction;
    ParticleSystem projectile;
    Collider projectileCollider;

    private void Start()
    {
        Destroy(gameObject, 5f);
        projectile = GetComponentInChildren<ParticleSystem>();
        projectileCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (!collided) transform.position += speed * Time.deltaTime * direction;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!collided && !other.gameObject.CompareTag("projectile") && !other.gameObject.CompareTag("Enemy"))
        {
            collided = true;

            StartCoroutine(Collide());
        }
    }

    IEnumerator Collide()
    {
        projectile.Stop();
        projectileCollider.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        yield return null;
    }
}
