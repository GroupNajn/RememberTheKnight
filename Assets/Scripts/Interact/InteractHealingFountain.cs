using System.Collections;
using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable, IInteractableUI
{
    private GameObject player;
    private PlayerManager playerManager;

    [SerializeField] GameObject lightObject;
    [SerializeField] private int healingCost = 5;
    private Light lightSource;

    private Collider interactCollider;
    [SerializeField] bool isExpended;

    private bool canShowUI = false;

    private int currentSoulCollect;

    private InteractUI_Controller interactUI_Controller;

    [Header("Charges")]
    [SerializeField] int chargesPerHeal = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        interactCollider = GetComponent<CapsuleCollider>();
        lightSource = lightObject.GetComponent<Light>();
        interactUI_Controller = GetComponent<InteractUI_Controller>();
    }
    // The cost to heal is currently hard coded to the value 5. 
    public void Interact()
    {
        currentSoulCollect = LootManager.instance.GetComponent<lootManager>().currentSoulCount;
        if (!isExpended && currentSoulCollect >= healingCost)
        {
            playerManager.GetCharges(chargesPerHeal);

            //playerManager.Heal(25f);
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
            data.InfoText = $"Let me consume {healingCost} souls to replenish a " +
            $"portion of your former self.";
            data.CanInteract = true;
        }
        else
        {
            data.InfoText = $"My well's essence is depleted.";
            data.CanInteract = false;
        }

            return data;
    }

    public void ShowUI()
    {
        if (!canShowUI && interactUI_Controller != null) return;

        interactUI_Controller.EnableCanvasObject();

    }

    public void HideUI()
    {
        
        interactUI_Controller.DisableCanvas();
    }

    public void SetLookedAt(bool value)
    {
        canShowUI = value;


    }



}
