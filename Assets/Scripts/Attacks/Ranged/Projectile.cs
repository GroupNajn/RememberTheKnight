using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// Controls projectile movement, collision detection,
/// particle effects and destruction behaviour.
/// </summary>
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

    /// <summary>
    /// Caches references and determines whether this projectile is an arrow.
    /// </summary>
    private void Awake()
    {
        isArrow = this.gameObject.name.Contains("Arrow");
        projectiles.AddRange(GetComponentsInChildren<ParticleSystem>());
        projectileCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        origin = transform.position;

       
        RuntimeManager.PlayOneShotAttached(flyingEvent, gameObject);
    }

    /// <summary>
    /// Moves the projectile along its assigned direction
    /// until a collision occurs.
    /// </summary>
    void Update()
    {
        if (!collided) transform.position += speed * Time.deltaTime * direction;
    }

    /// <summary>
    /// Detects valid collisions and starts the collision sequence.
    /// </summary>
    /// <param name="other">Collider entered by the projectile.</param>
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

    /// <summary>
    /// Stops particle effects, disables collisions,
    /// waits briefly if necessary and destroys the projectile.
    /// </summary>
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
