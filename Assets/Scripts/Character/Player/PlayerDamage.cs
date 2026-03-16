using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MaxHealth { get; set; }
    [HideInInspector] public int Health { get; set; }
    [HideInInspector] public bool CanTakeDamage { get; set; } = true;

    private void Start()
    {
        Health = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (CanTakeDamage && Health > 0)
        {
            Health -= damage;
            //CanTakeDamage = false;
            if (Health <= 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Debug.Log("DIE!");
    }
}