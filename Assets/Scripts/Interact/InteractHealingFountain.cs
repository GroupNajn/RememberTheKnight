using System.Collections;
using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable
{
    private PlayerStats playerStats;

    [SerializeField] GameObject lightObject;
    private Light lightSource;

    private Collider interactCollider;
    [SerializeField] bool isExpended;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        interactCollider = GetComponent<CapsuleCollider>();
        lightSource = lightObject.GetComponent<Light>();
    }

    public void Interact()
    {
        if (!isExpended)
        { 
            playerStats.Heal(25f);
            interactCollider.enabled = false;
            isExpended = true;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        var fog = lightObject.GetComponentInChildren<ParticleSystem>();
        fog.Stop();
        while (lightSource.intensity > 0.01f)
        {
            lightSource.intensity *= 0.9f;
            yield return new WaitForSeconds(0.05f);
        }
        lightObject.SetActive(false);
        yield return null;
    }

}
