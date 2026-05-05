using System.Collections;
using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable, IInteractableUIText
{
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

        PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

        if (InteractableSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!InteractableSaveSystem.HasInteracted(interactableID))
        {
            InteractableSaveSystem.SetInteracted(interactableID);

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
            data.InfoText = $"[F]: Replenish yor cup for {healingCost} souls.";
            data.CanInteract = true;
        }
        else
        {
            data.InfoText = $"My essence is depleted.";
            data.CanInteract = false;
        }

        return data;
    }
}