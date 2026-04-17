using System.Collections;
using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable
{
    private PlayerManager playerManager;

    [SerializeField] GameObject lightObject;
    [SerializeField] private int healingCost = 5;
    private Light lightSource;

    private Collider interactCollider;
    [SerializeField] bool isExpended;
    private int currentSoulCollect;
    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        interactCollider = GetComponent<CapsuleCollider>();
        lightSource = lightObject.GetComponent<Light>();

    }
    // The cost to heal is currently hard coded to the value 5. 
    public void Interact()
    {
        currentSoulCollect = LootManager.instance.GetComponent<Loot_System>().currentSoulCount;
        if (!isExpended && currentSoulCollect >= healingCost)
        { 
            playerManager.Heal(25f);
            interactCollider.enabled = false;
            isExpended = true;
            StartCoroutine(FadeOut());
            Event_System.instance?.OnSoulsSpent.Invoke(healingCost);
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
