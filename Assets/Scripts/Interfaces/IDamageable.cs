using System.Collections;
public interface IDamageable
{
    // Made by Lukas and Anton A 2026-03-06
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public bool CanTakeDamage { get; set; }

    //IEnumerator DamageCoolDown(int damageDelay); // Uncomment if you want to use a cooldown for taking damage, but currently not used in the project
    public void TakeDamage(int damage);
    public void Death();
}