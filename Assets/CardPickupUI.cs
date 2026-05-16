
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPickupUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tierTMP;
    [SerializeField] TextMeshProUGUI familyTMP;
    [SerializeField] TextMeshProUGUI infoBoxTMP;
    [SerializeField] CardData cardData;
    [SerializeField] AnimationCurve bounceCurve;
    [SerializeField] RectTransform pickupWindowTransform;
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] GameObject UIManager;
    [SerializeField] List<GameObject> disableGameObjects;
    [SerializeField] GameObject imageObject;
    [SerializeField] Color CommonColor;
    [SerializeField] Color UncommonColor;
    [SerializeField] Color RareColor;
    [SerializeField] Color EpicColor;
    [SerializeField] Color LegendaryColor;

    private UIManager uiManager;
    public bool isNormalScale { get; private set; }


    private Vector3 targetScale;
    [SerializeField] private float duration = 1f;

    [Header ("SFX")]
    public EventReference cardPickupEvent;
    public EventReference cardSacreficeEvent;

    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        targetScale = new Vector3(1, 1, 1);
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    private void OnEnable()
    {
        SetFamilyText();
        SetTierText();
        SetInfoBoxText();
        SetDisplayImage();
        DisableGameObjects();
        StartCoroutine(ScaleBouncePickUpWindow());
    }

    void Update()
    {
        isNormalScale = pickupWindowTransform.localScale != Vector3.zero;
    }
    public void OnConfirmPickup()
    {
        LootManager lootManager = GameObject.Find("Loot_Manager").GetComponent<LootManager>();
        RuntimeManager.StudioSystem.setParameterByName("CardRarity", (float)lootManager.GetRarityFromTier(cardData.cardTier));
        RuntimeManager.PlayOneShot(cardPickupEvent);

        PlayerCollection collection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        gameObject.SetActive(false);
        collection.PickupCard(cardData);
        uiManager.UIMenuActive = false;
        uiManager.CheckUIState();
    }

    public void OnSacrificeCard()
    {
        RuntimeManager.PlayOneShot(cardSacreficeEvent);
        gameObject.SetActive(false);
        Event_System.instance.OnDroopMultipleSouls.Invoke(cardData);
        uiManager.UIMenuActive = false;
        uiManager.CheckUIState();
    }

    public void Close()
    {
        pickupWindowTransform.localScale = Vector3.zero;
    }

    public void Open()
    {
        pickupWindowTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// Animates the pick-up window's scale using a bounce effect over a set duration.
    /// </summary>
    /// <remarks>This coroutine should be started to visually animate the pick-up window with a bounce scaling
    /// effect. The window's scale is updated each frame according to the specified animation curve. At the end of the
    /// animation, the window's scale is reset and related buttons are enabled.</remarks>
    /// <returns>An enumerator that performs the scale animation when iterated.</returns>
    private IEnumerator ScaleBouncePickUpWindow()
    {
        float timer = 0f;
        // 300 x 450
        // 300 x 1.5
        // 350 x 525
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;
            float curvevalue = bounceCurve.Evaluate(t);

            pickupWindowTransform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        pickupWindowTransform.localScale = Vector3.one;
        EnableGameObjects();
    }

    private void DisableGameObjects()
    {
        foreach (GameObject obj in disableGameObjects)
        {
            obj.SetActive(false);
        }
    }

    private void EnableGameObjects()
    {
        foreach (GameObject obj in disableGameObjects)
        {
            obj.SetActive(true);
        }
    }
    /// <summary>
    /// Sets the current card data for the instance. 
    /// Use this inside of UIManager when deciding if CardPickupUI should be enabled.
    /// Failsafe to set the card and if it is set, enable CardPickupUI
    /// </summary>
    /// <param name="card">The card data to assign. Cannot be null.</param>
    /// <returns>true if the card data was set successfully; otherwise, false.</returns>
    public bool SetCardData(CardData card)
    {
        return (cardData = card) != null;
    }
    private void SetFamilyText()
    {
        StringBuilder familyText = new StringBuilder();
        familyText.AppendLine("Family");

        familyText.Append($"{cardData.cardFamily}");
        familyTMP.text = familyText.ToString();

    }

    private void SetTierText()
    {

        StringBuilder tierText = new StringBuilder();
        tierText.AppendLine("Rarity");

        LootManager manager = GameObject.Find("Loot_Manager").GetComponent<LootManager>();
        tierText.Append($"{manager.GetRarityFromTier(cardData.cardTier)}");
        tierTMP.text = tierText.ToString();
    }

    private void SetDisplayImage()
    {
        Image image = imageObject.GetComponent<Image>();
        image.sprite = cardData.cardImage;
    }

    private RarityTier GetRarityTier(Tier tier)
    {
        LootManager manager = GameObject.Find("Loot_Manager").GetComponent<LootManager>();


        RarityTier currentTier = manager.GetRarityFromTier(tier);
        switch (currentTier)
        {
            case RarityTier.Common:
                return RarityTier.Common;
            case RarityTier.Uncommon:
                return RarityTier.Uncommon;
            case RarityTier.Rare:
                return RarityTier.Rare;
            case RarityTier.Epic:
                return RarityTier.Epic;
            case RarityTier.Legendary:
                return RarityTier.Legendary;
            default:
                return RarityTier.Legendary;

        }

    }
    private void SetInfoBoxText()
    {
        StringBuilder stats = new StringBuilder();

        if (cardData.healthModifier > 0)
            stats.AppendLine($"Health + {cardData.healthModifier}");

        if (cardData.staminaModifier > 0)
            stats.AppendLine($"Stamina + {cardData.staminaModifier}");

        if (cardData.luckModifier > 0)
            stats.AppendLine($"Luck + {cardData.luckModifier}%");

        if (cardData.damageModifier > 0)
            stats.AppendLine($"Damage + {cardData.damageModifier * 100}%");

        if (cardData.critChance > 0)
            stats.AppendLine($"critical chance + {cardData.critChance}%");

        if (cardData.walkSpeedModifier > 0)
            stats.AppendLine($"walk speed + {cardData.walkSpeedModifier}");

        if (cardData.sprintSpeedModifier > 0)
            stats.AppendLine($"sprint speed + {cardData.sprintSpeedModifier}%");

        if (cardData.dodgeSpeedModifier > 0)
            stats.AppendLine($"dodge speed + {cardData.dodgeSpeedModifier}%");

        if (cardData.healModifier > 0)
            stats.AppendLine($" heal multiplier + {cardData.healModifier}%");

        if (cardData.knockbackModifier > 0)
            stats.AppendLine($"resistance + {cardData.knockbackModifier}%");

        if (cardData.weaponSize != Vector3.zero)
            stats.AppendLine($"weapon size + {cardData.weaponSize.y * 10}");

        infoBoxTMP.text = stats.ToString();
    }

}
