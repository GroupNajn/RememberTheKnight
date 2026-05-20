using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBoardCardSlot : MonoBehaviour
{
    [field: SerializeField] public bool isLocked { get; private set; }

    [SerializeField] private ShopBoard board;
    [SerializeField] public CardData CurrentCard;
    [SerializeField] private GameObject spawnedCard;

    [Header("Visual indicators")]
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private GameObject candleVisual;
    [SerializeField] private GameObject boardSlot;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private bool isAnimating;
    public float AnimationDuration => animationDuration;
    public bool IsAnimating => isAnimating;

    [SerializeField] private Vector3 startRot;
    [SerializeField] private Vector3 endRot;

    private bool isSelected;

    private void Awake()
    {
        if (selectedVisual != null)
            selectedVisual.SetActive(false);
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

        spawnedCard = board.CardBuilder.InstantiateCardWithoutScripts(card, this.transform);
    }

    public void RemoveCard()
    {
         if (spawnedCard != null)
        {
            Destroy(spawnedCard);
            spawnedCard = null;
        }

         CurrentCard = null;
    }

    public void AnimateSelectSpin(System.Action onComplete = null)
    {
        LeanTween.rotateAroundLocal(boardSlot, endRot, 360f, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void AnimateHoverRotation(System.Action onComplete = null)
    {
        LeanTween.rotateLocal(boardSlot, endRot, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            isAnimating = false;
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

    IEnumerator ShowVisuals(bool active)
    {
        yield return new WaitForSeconds(animationDuration);

        selectedVisual?.SetActive(active);
        candleVisual?.SetActive(active);
    }

    public void SetHoverVisual(bool active)
    {
        LeanTween.cancel(boardSlot);

        if (isSelected)
        {
            AnimateHoverRotation();
            return;
        }

        if (active)
            AnimateHoverRotation();
        else
            AnimateUnHoverRotation();
    }

    public void SetSelectedVisual(bool selected)
    {
        isSelected = selected;
        
        StartCoroutine(ShowVisuals(selected));
    }
    public void SetLocked(bool value)
    {
        isLocked = value;
    }
}
