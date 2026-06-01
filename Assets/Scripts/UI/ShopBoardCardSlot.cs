using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBoardCardSlot : MonoBehaviour
{
    /// <summary>
    /// Created by Anton and Henric 2026-04-28
    /// Initially created as a component to handle the individual card slots on the shop board, which are populated with cards that the player can purchase.
    /// These are used by the ShopBoard component to populate the shop board with cards, and also handle the selection and hovering of the cards on the shop board.
    /// 
    /// Changed by Anton 2026-04-30
    /// Added functionality to add posters instead of cards if the slot is locked, or card data is null.
    /// 
    /// Changed by Anton 2026-05-09
    /// Added the linked board slot functionality and added visual effects for hovering and selecting cards.
    /// 
    /// Changed by Anton 2026-05-18
    /// Added functionality for animations for hovering and selecting.
    /// 
    /// Changed by Anton 2026-05-20
    /// Added remove functionality to remove card from the slot after purchase.
    /// </summary>
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

        if (isLocked || card == null)
        {
            int randomIndex = Random.Range(0, board.RandomPosters.Count);
            GameObject randomPoster = board.RandomPosters[randomIndex];

            Vector3 positionOffset = randomPoster.transform.localPosition;

            GameObject instance = Instantiate(randomPoster, this.transform.position, this.transform.rotation, this.transform);
            instance.transform.localPosition = randomPoster.transform.localPosition;

            spawnedCard = instance;
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

    /// <summary>
    /// Animates with leantween tool to rotate card.
    /// Uses onComplete callback action to trigger any additional logic after the animation is complete, such as enabling visual indicators or allowing further interactions.
    /// </summary>
    /// <param name="onComplete"></param>
    public void AnimateHoverRotation(System.Action onComplete = null)
    {
        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.cardFlipEvent);
        LeanTween.rotateLocal(boardSlot, endRot, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            isAnimating = false;
            onComplete?.Invoke();
        });
    }
    public void AnimateUnHoverRotation(System.Action onComplete = null)
    {
        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.cardFlipEvent);

        LeanTween.rotateLocal(boardSlot, startRot, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    IEnumerator ShowVisuals(bool active)
    {
        yield return new WaitForSeconds(animationDuration);
        if(active) 
            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.shopSelectCardEvent);
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
