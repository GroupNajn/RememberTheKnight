using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDamageTrigger : MonoBehaviour
{
    public float particleDamage = 10f;
    public ParticleSystem particles;
    public List<ParticleCollisionEvent> collisionEvents;
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        collisionEvents = new();
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.layer != gameObject.layer)
        {
            if (!other.TryGetComponent<IDamageable>(out var damageable)) return;

            int collisionCount = particles.GetCollisionEvents(other, collisionEvents);
            for (int i = 0; i < collisionCount; i++)
            {
                damageable.TakeDamage(new(particleDamage), collisionEvents[i].intersection);
            }
        }
    }
}