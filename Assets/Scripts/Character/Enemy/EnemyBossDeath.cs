using System.Collections;
using UnityEngine;

/// <summary>
/// When the bosses health reaches zero plays a particle effect and spawns a reward pedistal and then destroys the gameObject
/// </summary>
/// <remarks>Author: Theo Johansson and Henric Nilsson</remarks>
public class EnemyBossDeath : MonoBehaviour, ITriggerable
{
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject bossPedestal;
    public void Trigger()
    {
        StartCoroutine(DeleteCorpse(Instantiate(deathParticles, transform.position, Quaternion.identity)));

    }
    IEnumerator DeleteCorpse(ParticleSystem particles)
    {
        EnablePedestal();

        particles.Play();
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => particles.IsAlive(true));
        Destroy(particles);
        Destroy(gameObject);
        //Event_System.instance.OnBossDeath?.Invoke();
    }
    private void EnablePedestal()
    {
        bossPedestal.SetActive(true);
    }
}
