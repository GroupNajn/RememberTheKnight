using FMODUnity;
using System.Collections;
using System.Text;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static IPickupable;


// Script Updated by Henric 2026-04-17
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlocked { get; private set; } = false;
    [field: SerializeField] public bool OverrideLockState { get; private set; } = false;

    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;


    [SerializeField] public CardData cardData;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image infoBoxImage;

    [SerializeField] private GameObject infoBox;
    [SerializeField] private GameObject outlineImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;
    private bool alwaysShowInfo;

    private string damageText;

    public float angle = 5f;
    public float speed = 0.4f;
    public float offset = 0f;
    float time;

    private Quaternion startRotation;

    private RectTransform rectTransform;
    private float baseRotationZ;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        baseRotationZ = rectTransform.localEulerAngles.z;
        IsSelected = false;


        cardImage = GetComponent<Button>().targetGraphic as Image;
        infoBoxImage = infoBox.GetComponent<Image>();
        // cardImage = GetComponent<Button>().targetGraphic();


        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        nameText = texts[0];
        statsText = texts[1];

        infoBox.SetActive(false);
        //cardImage.rectTransform.sizeDelta = this.rectTransform.sizeDelta;

    }
    public void Setup(CardData data, bool ShowInfo = false)
    {
        cardData = data;

        if (cardImage == null)
            cardImage = GetComponent<Image>();

        if (cardData != null)
        {
            cardImage.sprite = cardData.cardImage;
        }
        cardImage.rectTransform.sizeDelta = new Vector2(300, 100);
    }
    void Start()
    {
        startRotation = transform.localRotation;
        IsSelected = false;

        if (cardData != null && cardImage != null)
        {
            cardImage.sprite = cardData.cardImage;
        }
    }
    // Added IsSelected = false becuase the first time the card is started via 
    // UIManager it is set to false to default to that, once it's state has been updated once during the game.
    // It will no longe be reset to false. 

    private void OnEnable()
    {
        if (!IsUnlocked)
        {
            cardImage.color = lockedColor;
            infoBoxImage.color = lockedColor;
        }
        else
        {
            cardImage.color = unlockedColor;
            infoBoxImage.color = unlockedColor;
        }

        StartCoroutine(UnFlipCard());
    }

    private void Update()
    {
        if (OverrideLockState)
        {
            IsUnlocked = true;
        }

        //if (IsSelected)
        //{
        //    time++;

        //    float wiggle = Mathf.Sin((time + offset) * speed) * angle;
        //    rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, baseRotationZ + wiggle);

        //}
        //else
        //{
        //    rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, baseRotationZ);
        //}
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
        outlineImage.SetActive(selected);
    }

    public void SetUnlocked(bool unlockable)
    {
        this.IsUnlocked = unlockable;
    }

    public void CheckStatsForString()
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

        if (cardData.luckModifier < 0)
            stats.AppendLine($"Luck {cardData.luckModifier}% you are unlucky");

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

        statsText.text = stats.ToString();
    }

    public void OnSelectedCard()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameText.text = cardData.cardName;

        CheckStatsForString();
        // infoBox.SetActive(true);
        StartCoroutine(FlipCard());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // infoBox.SetActive(false);
        
        StartCoroutine(UnFlipCard());
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            nameText.text = cardData.cardName;

            
            CheckStatsForString();
            // infoBox.SetActive(true);
            StartCoroutine(FlipCard());
            
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            
            StartCoroutine(UnFlipCard());
            
        }
    }

    IEnumerator FlipCard()
    {
        float time = 0f;
        float cardFlipDuration = 0.15f;
        Vector3 originalScale = cardImage.rectTransform.localScale;
        while (time < cardFlipDuration)
        {
            time += Time.deltaTime;
            float t = time / cardFlipDuration;
            cardImage.rectTransform.localScale = Vector3.Lerp(originalScale, new Vector3(0.01f, 1, 1), t);
            yield return null;

        }
        cardImage.rectTransform.localScale = new Vector3(0.01f, 1, 1);

        infoBox.SetActive(true);
        time = 0f;
        while (time < cardFlipDuration)
        {
            time += Time.deltaTime;

            float t = time / cardFlipDuration;
            cardImage.rectTransform.localScale = Vector3.Lerp(new Vector3(0.01f, 1, 1), new Vector3(1, 1, 1), t);
            yield return null;

        }
        cardImage.rectTransform.localScale = new Vector3(1, 1, 1);
    }


    IEnumerator UnFlipCard()
    {
        //yield return StartCoroutine(FlipCard());

        yield return new WaitForSeconds(0.2f);    // Small delay before Allowing flipping back, adjust as needed

        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.cardFlipEvent);

        float time = 0f;
        float cardFlipDuration = 0.15f;
        Vector3 originalScale = cardImage.rectTransform.localScale;

        while (time < cardFlipDuration)
        {
            time += Time.deltaTime;
            float t = time / cardFlipDuration;
            cardImage.rectTransform.localScale = Vector3.Lerp(originalScale, new Vector3(0.01f, 1, 1), t);
            yield return null;

        }
        cardImage.rectTransform.localScale = new Vector3(0.01f, 1, 1);

        infoBox.SetActive(false);

        time = 0f;
        while (time < cardFlipDuration)
        {
            time += Time.deltaTime;

            float t = time / cardFlipDuration;
            cardImage.rectTransform.localScale = Vector3.Lerp(new Vector3(0.01f, 1, 1), new Vector3(1, 1, 1), t);
            yield return null;

        }
        cardImage.rectTransform.localScale = new Vector3(1, 1, 1);
    }
}