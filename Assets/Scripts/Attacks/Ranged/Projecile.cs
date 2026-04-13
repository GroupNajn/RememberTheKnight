using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projecile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool collided;

    public float speed;
    public Vector3 direction;
    List<ParticleSystem> projectiles = new();
    Collider projectileCollider;
    Vector3 origin;


    private void Awake()
    {
        //Destroy(gameObject, 5f);
        projectiles.AddRange(GetComponentsInChildren<ParticleSystem>());
        projectileCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        if (!collided) transform.position += speed * Time.deltaTime * direction;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!collided && !other.gameObject.CompareTag("Projectile") && !other.gameObject.CompareTag("Enemy"))
        {
            collided = true;

            StartCoroutine(Collide());
        }
    }

    IEnumerator Collide()
    {
        projectiles.ForEach(projectile =>
        {
            if (projectile != null) projectile.Stop();
        });
        projectileCollider.enabled = false;
        if (Vector3.Distance(origin, transform.position) > 1f)
        {
            yield return new WaitForSeconds(.25f);
        }
        Destroy(gameObject);
        yield return null;
    }
}
