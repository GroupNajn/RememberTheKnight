using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CardUnlockUI : AutoSelectFirstButtonOnEnable
{
    [Header("TextMeshPros")]
    [SerializeField] TextMeshProUGUI rememberTMP;
    [SerializeField] TextMeshProUGUI familyTMP;
    [SerializeField] TextMeshProUGUI tierTMP;
    [SerializeField] TextMeshProUGUI cardInfoTMP;
    [SerializeField] TextMeshProUGUI errorTMP;
    [Header("GeneralData")]
    [SerializeField] CardData cardData;
    [SerializeField] AnimationCurve bounceCurve;
    [SerializeField] private UIManager uiManager;

    [Header("Data to change")]
    [SerializeField] List<GameObject> disableGameObjects;
    [SerializeField] RectTransform unlockWindowTransform;
    [SerializeField] GameObject imageObject;

    [SerializeField] RectTransform rescaleRect;

    public bool isNormalScale { get; private set; }
    private Vector3 targetScale;

    [SerializeField] private float duration = 1f;
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
        isNormalScale = unlockWindowTransform.localScale != Vector3.zero;
    }

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

            unlockWindowTransform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        unlockWindowTransform.localScale = Vector3.one;
        LeanTween.scale(rescaleRect, new Vector3(0.95f, 0.95f, 0.95f), 1.5f).setEaseInBack().setLoopPingPong().setIgnoreTimeScale(true);
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

    public void OnRememberCard()
    {
        CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        InteractSoulDonation donation = GameObject.FindWithTag("DonationWell").GetComponent<InteractSoulDonation>();
        gameObject.SetActive(false);
        cardSystem.UnlockCardFromDonation(cardData);
        PlayerPrefsSaveSystem.SetSaveState(cardData.cardID);
        if(cardData.cardTier >= Tier.X)
        {
            uiManager.UIMenuActive = false;
            uiManager.CheckUIState();
        } 
        donation.SetNextCard();
        donation.SetNextCardCost();
        donation.SetSoulsDonatedSinceLastToFamily();
        GameDataUpdater.instance.SetSoulsCostInNextCard(GameObject.Find("Player").GetComponent<PlayerCollection>().playerContract);

        uiManager.UIMenuActive = false;
        uiManager.CheckUIState();

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

  

    public void Close()
    {
        unlockWindowTransform.localScale = Vector3.zero;
    }

    public void Open()
    {
        unlockWindowTransform.localScale = Vector3.one;
    }

    private void SetInfoBoxText()
    {
        StringBuilder stats = new StringBuilder();

        if (cardData.healthModifier > 0)
            stats.AppendLine($"Health + {cardData.healthModifier}");

        if (cardData.healRegeneraion > 0)
            stats.AppendLine($"Health regen + {cardData.healRegeneraion}");

        if (cardData.staminaModifier > 0)
            stats.AppendLine($"Stamina + {cardData.staminaModifier}");

        if (cardData.staminaRegeneraion > 0)
            stats.AppendLine($"Stamina regen + {cardData.staminaRegeneraion}");

        if (cardData.luckModifier > 0)
            stats.AppendLine($"Luck + {cardData.luckModifier}%");

        if (cardData.damageModifier > 0)
            stats.AppendLine($"Damage + {cardData.damageModifier * 100}%");

        if (cardData.critChance > 0)
            stats.AppendLine($"Crit chance + {cardData.critChance}%");

        if (cardData.dodgeSpeedModifier > 0)
            stats.AppendLine($"Dodge speed + {cardData.dodgeSpeedModifier}%");

        if (cardData.healModifier > 0)
            stats.AppendLine($"Heal multiplier + {cardData.healModifier}%");

        if (cardData.knockbackModifier > 0)
            stats.AppendLine($"Resistance + {cardData.knockbackModifier}%");

        if (cardData.actionSpeedModifier > 0f)
            stats.AppendLine($"Speed + {cardData.actionSpeedModifier * 100}%");

        if (cardData.weaponSize != Vector3.zero)
            stats.AppendLine($"Weapon size + {cardData.weaponSize.y * 10}");


        cardInfoTMP.text = stats.ToString();
    }


}
