using FMODUnity;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BossCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [field: SerializeField] public bool IsSelected { get; private set; }
    [field: SerializeField] public bool IsUnlocked { get; private set; } = false;
    [field: SerializeField] public bool OverrideLockState { get; private set; } = false;

    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color familyColor = Color.white;

    [SerializeField] private BossCardSelectUI cardSelectUI;
    [SerializeField] public CardData cardData;
    [SerializeField] private Image cardImage;

    [SerializeField] private GameObject infoBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;
    private TextAlignmentOptions nameAlignment = TextAlignmentOptions.Center;
    private TextAlignmentOptions statsAlignment = TextAlignmentOptions.Top;

    private string defaultNameText;
    private string defaultStatsText;
    private Color defaultColor = Color.black;
    private TextAlignmentOptions defaultNameAlignment = TextAlignmentOptions.Center;
    private TextAlignmentOptions defaultStatsAlignment = TextAlignmentOptions.TopLeft;

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
        cardSelectUI = GetComponentInParent<BossCardSelectUI>();

        defaultNameText = nameText.text;
        defaultStatsText = statsText.text;

        rectTransform = GetComponent<RectTransform>();
        baseRotationZ = rectTransform.localEulerAngles.z;
        IsSelected = false;
    }
    public void Setup(CardData data, bool ShowInfo = false)
    {
        cardData = data;

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
            IsUnlocked = true;
        }

        if (!IsUnlocked)
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

        if (selected)
        {
            SetCardInfo();
        }
        else
        {
            ShowDefaultInfo();
        }
    }

    public void SetUnlocked(bool unlockable)
    {
        this.IsUnlocked = unlockable;
    }

    public void SetCardInfo()
    {
        nameText.alignment = nameAlignment;
        statsText.alignment = statsAlignment;
        nameText.text = cardData.name;
        nameText.color = familyColor;
        CheckStatsForString();
    }

    public void ShowDefaultInfo()
    {
        nameText.alignment = defaultNameAlignment;
        statsText.alignment = defaultStatsAlignment;
        nameText.color = defaultColor;
        statsText.color = defaultColor;
        nameText.text = defaultNameText;
        statsText.text = defaultStatsText;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCardInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardSelectUI.CurrentSelectedCard != null)
        {
            cardSelectUI.CurrentSelectedCard.SetCardInfo();
        }
        else
        {
            ShowDefaultInfo();
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            SetCardInfo();
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (InputManager.Instance.usingGamepad)
        {
            if (cardSelectUI.CurrentSelectedCard != null)
            {
                cardSelectUI.CurrentSelectedCard.SetCardInfo();
            }
            else
            {
                ShowDefaultInfo();
            }
        }
    }
}
