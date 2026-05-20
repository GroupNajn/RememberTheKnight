using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class Projectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool collided;
    private bool isArrow;
    public float speed;
    public Vector3 direction;
    List<ParticleSystem> projectiles = new();

    [field: SerializeField] public EnemyWeaponManager enemyWeaponManager { get; set; }

    Collider projectileCollider;
    Vector3 origin;
    [Header("SFX")]
    public EventReference flyingEvent;


    private void Awake()
    {
        isArrow = this.gameObject.name.Contains("Arrow");
        projectiles.AddRange(GetComponentsInChildren<ParticleSystem>());
        projectileCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        origin = transform.position;

        if(isArrow)
        RuntimeManager.PlayOneShotAttached(flyingEvent, gameObject);
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
            if (isArrow && other.gameObject.CompareTag("Player"))
            {
                Vector3 contactPoint = other.ClosestPoint(transform.position);
                GameObject.FindWithTag("Player").GetComponent<PlayerVFX>().PlayArrowVFX(contactPoint);
            }
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
