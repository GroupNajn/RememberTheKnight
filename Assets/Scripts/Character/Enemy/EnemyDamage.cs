using Unity.Behavior;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(ITriggerable))]
public class EnemyDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private Event_System EventSystem;
    // Made by Lukas and Anton A 2026-03-06
    [field: SerializeField] public int MaxHealth { get; set; }
    [HideInInspector] public int Health { get; set; }
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;
    private ITriggerable onDeath;
    public Vector3 Position => position;
    Vector3 position;
    float damageCooldownTimer = 1;
    [SerializeField] float damageCooldown = 1;

    public void TakeDamage(int damage)
    {
        if (CanTakeDamage && Health > 0)
        {
            Debug.Log($"Taking damage{damage}");

            Health -= damage;
            Debug.Log($"Health {Health}/{MaxHealth}");
            CanTakeDamage = false;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Transform transform = GetComponent<Transform>();
        position = transform.position;
        EventSystem.OnEnemyKilled?.Invoke(this);
        onDeath?.Trigger();
       
    }

    // TO BE REMOVED OR CHANGED
    void Update()
    {
        if (!CanTakeDamage)
        {
            damageCooldownTimer -= Time.deltaTime;
            if (damageCooldownTimer <= 0)
            {
                CanTakeDamage = true;
                damageCooldownTimer = damageCooldown;
            }
        }
    }

    private void Start()
    {
        Health = MaxHealth;
        onDeath = GetComponent<ITriggerable>();
        EventSystem.OnEnemySpawn?.Invoke(this);

    }



}