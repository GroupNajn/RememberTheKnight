using System.Collections.Generic;
using UnityEngine;

public class ShopBoardCardSlot : MonoBehaviour
{
    [field: SerializeField] public bool isLocked { get; private set; }

    [SerializeField] private ShopBoard board;
    [SerializeField] public CardData CurrentCard;

    [Header("Visual indicators")]
    [SerializeField] private GameObject hoverVisual;
    [SerializeField] private GameObject candleVisual;
    [SerializeField] private GameObject boardSlot;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private bool isAnimating;

    [SerializeField] private Vector3 startRot;
    [SerializeField] private Vector3 endRot;

    private bool isSelected;

    private void Awake()
    {
        if (hoverVisual != null)
            hoverVisual.SetActive(false);
    }

    public void SetCard(CardData card)
    {
        CurrentCard = card;

        if (isLocked)
        {
            int randomIndex = Random.Range(0, board.RandomPosters.Count);
            GameObject randomPoster = board.RandomPosters[randomIndex];

            Vector3 positionOffset = randomPoster.transform.localPosition;

            GameObject instance = Instantiate(randomPoster, this.transform.position, this.transform.rotation, this.transform);
            instance.transform.localPosition = randomPoster.transform.localPosition;

            return;
        }

        board.CardBuilder.InstantiateCardWithoutScripts(card, this.transform);
    }

    public void AnimateHoverRotation(System.Action onComplete = null)
    {
        LeanTween.rotateLocal(boardSlot, endRot, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void AnimateUnHoverRotation(System.Action onComplete = null)
    {
        LeanTween.rotateLocal(boardSlot, startRot, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void SetHoverVisual(bool active)
    {
        if (isSelected)
            return;

        if (active)
        {
            AnimateHoverRotation(() =>
            {
                // Enable Info
            });
        }
        else
        {
            AnimateUnHoverRotation(() =>
            {
                // Disable info
            });
        }
    }

    public void SetSelectedVisual(bool selected)
    {
        isSelected = selected;

        if (hoverVisual != null)
            hoverVisual.SetActive(selected);
    }
    public void SetLocked(bool value)
    {
        isLocked = value;
    }
}
