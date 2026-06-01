using System.Collections;
using UnityEngine;


public class EnemyBossDeath : MonoBehaviour, ITriggerable
{
    [SerializeField] ParticleSystem deathParticles;
    public void Trigger()
    {
        StartCoroutine(DeleteCorpse(Instantiate(deathParticles, transform.position, Quaternion.identity)));

    }
    IEnumerator DeleteCorpse(ParticleSystem particles)
    {
        particles.Play();
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => particles.IsAlive(true));
        Destroy(particles);
        Destroy(gameObject);
        Event_System.instance.OnBossDeath?.Invoke();
    }
}
