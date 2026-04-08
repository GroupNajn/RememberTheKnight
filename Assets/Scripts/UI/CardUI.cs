using UnityEngine;

public class CardUI : MonoBehaviour
{
    [field: SerializeField] public bool IsSelected { get; private set; }

    [SerializeField] public CardData cardData;
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
    }
    void Start()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {

        if (IsSelected)
        {
            time++;

            float wiggle = Mathf.Sin((time + offset) * speed) * angle;
            rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x,rectTransform.localEulerAngles.y,baseRotationZ + wiggle);

        }
        else
        {
            rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x,rectTransform.localEulerAngles.y, baseRotationZ);
        }
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
        Debug.Log($"Card {(selected ? "Selected" : "Deselected")}");
    }
}
