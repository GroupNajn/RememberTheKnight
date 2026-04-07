using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;

public class CardSelectionUI : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerUIManager playerUIManager;

    [SerializeField] ScrollRect cardScrollRect;
    [SerializeField] float ScrollAmount;

    [SerializeField] int maxCardsSelected = 4;
    [SerializeField] int currentCardsSelected = 0;
    [SerializeField] TextMeshProUGUI errorText;

    private float errorTimer = 0f;
    private float fadeDuration = 0.5f;
    bool fading = false;
    bool errorActive = false;

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerUIManager = GetComponentInParent<PlayerUIManager>();

        if (cardScrollRect == null)
            cardScrollRect = playerUIManager.GetComponentInChildren<ScrollRect>();

        errorText.gameObject.SetActive(false);


        playerUIManager.CloseCardSelectUI();
    }

    private void OnEnable()
    {
        currentCardsSelected = 0;

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            CardUI card = button.GetComponent<CardUI>();
            if (card == null) continue;

            if (card.IsSelected)
            {
                currentCardsSelected++;
            }

            Button btn = button;
            btn.onClick.AddListener(() => OnCardSelect(btn));
        }
    }

    public void OnArrowUp()
    {
        cardScrollRect.verticalNormalizedPosition += ScrollAmount;
    }

    public void OnArrowDown()
    {
        cardScrollRect.verticalNormalizedPosition -= ScrollAmount;
    }

    public void OnCardSelect(Button button)
    {
        CardUI card = button.GetComponent<CardUI>();

        if (card.IsSelected)
        {
            card.SetSelected(false);
            currentCardsSelected--;
            return;
        }

        if (currentCardsSelected >= maxCardsSelected)
        {


            if (!errorActive)
            {
                ShowError($"You can only select {maxCardsSelected} cards!", 5f);
            }

            return;
        }

        card.SetSelected(true);
        currentCardsSelected++;
        Debug.Log("CLICK FUNKAR: " + button.name);



    }
    private void Update()
    {
        if (!errorActive) return;

        errorTimer -= Time.unscaledDeltaTime;

        if (errorTimer <= fadeDuration)
        {
            fading = true;

            float alpha = Mathf.Clamp01(errorTimer / fadeDuration);
            errorText.alpha = alpha;
        }

        if (errorTimer <= 0f)
        {
            errorText.gameObject.SetActive(false);
            errorActive = false;
            fading = false;
            errorText.alpha = 1f;
        }
    }

    private void ShowError(string message, float duration)
    {
        errorText.text = message;
        fadeDuration = duration * 0.25f; // Fade out over the last 25% of the duration
        errorText.alpha = 1f;
        errorText.gameObject.SetActive(true);
        errorActive = true;
        errorTimer = duration;
    }
}
