using System.Collections;
using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable, IInteractableUIText
{
    /// <summary>
    /// Created by Anton and Theo 2026-04-13
    /// Initially created with minor effects and interactale features. Also with the light/particle effect.
    /// 
    /// Changed by Anton 2026-05-06
    /// Added saved data to the interactable, so that a one time effect can be played the first time the player interacts, and not played again after that.
    /// </summary>

    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    private GameObject player;
    private PlayerManager playerManager;

    [SerializeField] GameObject lightObject;
    [SerializeField] private int healingCost = 5;
    private Light lightSource;

    private Collider interactCollider;
    [SerializeField] bool isExpended;

    private int currentSoulCollect;

    [Header("Charges")]
    [SerializeField] int chargesPerHeal = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        interactCollider = GetComponent<CapsuleCollider>();
        lightSource = lightObject.GetComponent<Light>();

        if (PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            PlayerPrefsSaveSystem.SetSaveState(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }

        currentSoulCollect = LootManager.instance.GetComponent<Loot_System>().currentSoulCount;
        if (!isExpended && currentSoulCollect >= healingCost)
        {
            playerManager.GetCharges(chargesPerHeal);

            interactCollider.enabled = false;
            isExpended = true;
            StartCoroutine(FadeOut());
            Event_System.instance?.OnSoulsSpent.Invoke(healingCost);
            //player.GetComponent<PlayerVFX>().PlayHealVFX();
        }
    }

    /// <summary>
    /// IEnumerator that fade the light source and particle effect out after interaction.
    /// </summary>
    /// <returns></returns>
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

    public InteractableUIData GetUIData()
    {
        var data = new InteractableUIData();

        if (!isExpended)
        {
            data.CanInteract = true;
            data.InfoText = $"Replenish your cup for {healingCost} souls.";
        }
        else
        {
            data.CanInteract = false;
            data.InfoText = $"My essence is depleted.";
        }

        return data;
    }
}