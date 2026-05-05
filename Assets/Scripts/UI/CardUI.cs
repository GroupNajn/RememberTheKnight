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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Mouse entered button {cardData.cardName}");
        infoBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Mouse exited button {cardData.cardName}");
        infoBox.SetActive(false);
    }


}
