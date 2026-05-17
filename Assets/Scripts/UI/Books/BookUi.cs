using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BookUi : MonoBehaviour
{
    //made by Michaëla 2026-04-19
    //Updated by Anton 2026-05-16

    // Todo - make when pressing tab buttons keep pages on same page as now back does not work if pressed tab if pages stats has only one page.
    [Header("Pages")]
    [SerializeField] private BookPageUI leftPage;
    [SerializeField] private BookPageUI rightPage;

    [Header("Text Book Input")]
    private string bookText;
    [SerializeField] private int charsPerPage = 300;

    [Header("Tab Buttons")]
    [SerializeField] private Button statsButton;
    [SerializeField] private Button cardsButton;
    [SerializeField] private Button loreButton;
    [SerializeField] private BookMark statsTab;
    [SerializeField] private BookMark cardsTab;
    [SerializeField] private BookMark loreTab;

    [Header("Data")]
    [SerializeField] private List<LoreEntry> allLoreEntries = new();
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LoreManager loreManager;
    [SerializeField] private UIManager uiManager;

    [Header("Animation")]
    [SerializeField] private RectTransform movingBook;
    [SerializeField] private Transform movingPage;
    [SerializeField] private float animationDurationMoving = 1f;
    [SerializeField] private float animationDurationOpening = 0.5f;
    [SerializeField] private AnimationCurve bounceCurve;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 targetPos;

    [SerializeField] private float rotationAngle;
    [SerializeField] private Vector3 closedRotation;

    private List<PageData> statsPages = new();
    private List<PageData> cardPages = new();
    private List<PageData> lorePages = new();

    private List<PageData> currentPages = new();

    private int currentIndex = 0;

    public enum BookTabEnum
    {
        Stats,
        Cards,
        Lore
    }

    private BookTabEnum currentTab = BookTabEnum.Stats;

    public void OnEnable()
    {
        SetClosedInstant();
        BaseBookSetup(playerStats);
        StartCoroutine(ScaleBouncePickUpWindow(() =>
        {
            AnimateMove(() =>
            {
                AnimateOpen();
            });
        }));
    }

    public void OnDisable()
    {
        ResetBookState();
    }
    public void ResetBookState()
    {
        LeanTween.cancel(movingPage.gameObject);
        LeanTween.cancel(movingBook.gameObject);

        movingPage.localEulerAngles = closedRotation;
        movingBook.localPosition = startPos;
    }
    public void BaseBookSetup(PlayerStats stats)
    {
        BuildStatPages(stats);
        BuildCardPages();
        BuildLorePages();

        OpenTab(BookTabEnum.Stats);
        UpdateTabButtons();
    }

    public void SpecificBookSetup(PlayerStats stats, BookTabEnum tab)
    {
        BuildStatPages(stats);
        BuildCardPages();
        BuildLorePages();

        OpenTab(tab);
        UpdateTabButtons();
    }

    public void BuildStatPages(PlayerStats stats)
    {
        statsPages.Clear();

        statsPages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats
        });
    }

    public void BuildCardPages()
    {
        cardPages.Clear();
        List<CardData> allCards = new List<CardData>();

        allCards.AddRange(playerCollection.ReturnPermanentCardCollection());
        allCards.AddRange(playerCollection.ReturnTempCardCollection());

        for (int i = 0; i < allCards.Count; i += 4)
        {
            cardPages.Add(new PageData
            {
                type = PageData.PageType.Cards,
                cards = allCards.GetRange(i, Mathf.Min(4, allCards.Count - i))
            });
        }
    }

    public void BuildLorePages()
    {
        lorePages.Clear();
        foreach (var entry in allLoreEntries) 
        {
            bool unlocked = loreManager.IsLoreUnlocked(entry.id);
            var entryPages = entry.GetPages(charsPerPage);
            foreach (var page in entryPages)
            {
                lorePages.Add(new PageData
                {
                    type = PageData.PageType.Lore,
                    loreText = unlocked ? page : "???"
                });
            }
        }
    }

    public void ChangeTab(BookTabEnum tab)
    {
        AnimateClose(() =>
        {
            OpenTab(tab);

            AnimateOpen();
        });
    }

    public void OpenTab(BookTabEnum tab)
    {
        currentTab = tab;
        currentIndex = 0;

        switch (tab)
        {
            case BookTabEnum.Stats:
                currentPages = statsPages;
                break;
            case BookTabEnum.Cards:
                currentPages = cardPages;
                break;
            case BookTabEnum.Lore:
                currentPages = lorePages;
                break;
        }

        UpdateTabButtons();

        statsTab.SetSelected(currentTab == BookTabEnum.Stats);
        cardsTab.SetSelected(currentTab == BookTabEnum.Cards);
        loreTab.SetSelected(currentTab == BookTabEnum.Lore);

        ShowPages();
    }

    // Display current pages
    public void ShowPages()
    {
        if(currentPages == null || currentPages.Count == 0)
        {
            leftPage.gameObject.SetActive(false);
            rightPage.gameObject.SetActive(false);
            return;
        }

        // left 
        if (currentIndex < currentPages.Count)
        {
            leftPage.gameObject.SetActive(true);
            leftPage.Setup(currentPages[currentIndex]);
        }
        else
        {
            leftPage.gameObject.SetActive(false);
        }

        // Right
        if (currentIndex + 1 < currentPages.Count)
        {
            rightPage.gameObject.SetActive(true);
            rightPage.Setup(currentPages[currentIndex + 1]);
        }
        else
        {
            rightPage.gameObject.SetActive(false);
        }
    }

    // Flip forward
    public void NextPage()
    {
        if (currentPages == null)
            return;

        if (currentIndex + 2 < currentPages.Count)
        {
            currentIndex += 2;
            ShowPages();
        }
    }

    // Flip backward
    public void PrevPage()
    {
        if(currentPages == null)
            return;

        if (currentIndex - 2 >= 0)
        {
            currentIndex -= 2;
            ShowPages();
        }
    }

    public void CloseBook()
    {
        ResetBookState();

        uiManager.CloseBookUI();
    }

    // Tab buttons
    // go to the first page of the respective section, if it exists. If not, do nothing (or show a message)
    public void GoToStats()
    {
        ChangeTab(BookTabEnum.Stats);
    }

    public void GoToCards()
    {
        ChangeTab(BookTabEnum.Cards);
    }

    public void GoToLore()
    {
        if (lorePages.Count > 0)
            ChangeTab(BookTabEnum.Lore);
        
    }

    // Enable or disable tab buttons based on whether their respective pages exist
    private void UpdateTabButtons()
    {
        statsButton.interactable = currentTab != BookTabEnum.Stats;
        cardsButton.interactable = currentTab != BookTabEnum.Cards;
        loreButton.interactable = currentTab != BookTabEnum.Lore;
    }

    // Animations for book opening and closing

    public void SetClosedInstant()
    {
        movingBook.localPosition = startPos;
        movingPage.localEulerAngles = closedRotation;
    }

    public void AnimateMove(Action onComplete = null)
    {
        LeanTween.moveLocal(movingBook.gameObject, targetPos, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void AnimateOpen()
    {
        LeanTween.rotateAroundLocal(movingPage.gameObject, Vector3.forward, rotationAngle, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true);
    }

    public void AnimateClose(Action onComplete = null)
    {
        LeanTween.rotateAroundLocal(movingPage.gameObject, Vector3.forward, -rotationAngle, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private IEnumerator ScaleBouncePickUpWindow(Action onComplete = null)
    {
        float timer = 0f;
        // 300 x 450
        // 300 x 1.5
        // 350 x 525
        while (timer < animationDurationMoving)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / animationDurationMoving;
            float curvevalue = bounceCurve.Evaluate(t);

            movingBook.transform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        movingBook.transform.localScale = Vector3.one;
        //LeanTween.scale(rescaleRect, new Vector3(0.95f, 0.95f, 0.95f), 1.5f).setEaseInBack().setLoopPingPong().setIgnoreTimeScale(true);

        onComplete?.Invoke();
    }
}
