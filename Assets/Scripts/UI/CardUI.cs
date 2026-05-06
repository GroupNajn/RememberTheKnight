using System.Text;
using TMPro;
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


        cardImage = GetComponent<Image>();

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();

        nameText = texts[0];
        statsText = texts[1];

        infoBox.SetActive(false);

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
            infoBox.SetActive(true);
        }
        else
        {
            Debug.Log("this card was locked");
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoBox.SetActive(false);
    }


}
