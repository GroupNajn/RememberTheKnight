using System.Collections;
using UnityEngine;


public class EnemyBossDeath : MonoBehaviour, ITriggerable
{
    [SerializeField] ParticleSystem deathParticles;
    public void Trigger()
    {
        StartCoroutine(DeleteCorpse(Instantiate(deathParticles, transform.position, Quaternion.identity, transform)));

    }
    IEnumerator DeleteCorpse(ParticleSystem particles)
    {
        particles.Play();
        yield return new WaitUntil(() => !particles.isPlaying);
        Destroy(particles);
        Destroy(gameObject);
    }
}
