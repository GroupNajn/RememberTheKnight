using System.Collections;
using System.Text;
using TMPro;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Script Updated by Henric 2026-04-17
public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlockable { get; private set; } = false;
    [field: SerializeField] public bool OverrideLockState { get; private set; } = false;

    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;


    [SerializeField] public CardData cardData;
    [SerializeField] private Image cardImage;

    [SerializeField] private GameObject infoBox;
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


    private void Update()
    {
        if (OverrideLockState)
        {
            IsUnlockable = true;
        }

        if (!IsUnlockable)
        {
            cardImage.color = lockedColor;
        }
        else
        {
            cardImage.color = unlockedColor;
        }

        if (IsSelected)
        {
            time++;

            float wiggle = Mathf.Sin((time + offset) * speed) * angle;
            rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, baseRotationZ + wiggle);

        }
        else
        {
            rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, baseRotationZ);
        }
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
    }

    public void SetUnlockable(bool unlockable)
    {
        this.IsUnlockable = unlockable;
    }

    public void CheckStatsForString()
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

        statsText.text = stats.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        nameText.text = cardData.cardName;

        if (IsUnlockable)
        {
            CheckStatsForString();
            // infoBox.SetActive(true);
            StartCoroutine(FlipCard());
        }
        else
        {
            Debug.Log("this card was locked");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // infoBox.SetActive(false);
        if (IsUnlockable)
        {
            StartCoroutine(UnFlipCard());
        }

    }
    public void ToggleInfo()
    {
        if (cardData == null)
            return;

        nameText.text = cardData.cardName;
        CheckStatsForString();

        if (cardData.cardFamily == CardFamily.Cups)
        {
            ColorUtility.TryParseHtmlString("#5F2828", out var darkRed);
            infoBox.GetComponent<Image>().color = darkRed;
        }
        else if (cardData.cardFamily == CardFamily.Swords)
        {
            ColorUtility.TryParseHtmlString("#4B4A53", out var gray);
            infoBox.GetComponent<Image>().color = gray;
        }
        else if (cardData.cardFamily == CardFamily.Pentacles)
        {
            ColorUtility.TryParseHtmlString("#DAD232", out var yellow);
            infoBox.GetComponent<Image>().color = yellow;
        }
        else if (cardData.cardFamily == CardFamily.Wands)
        {
            ColorUtility.TryParseHtmlString("#435F28", out var green);
            infoBox.GetComponent<Image>().color = green;
        }
        infoBox.SetActive(!infoBox.activeSelf);
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