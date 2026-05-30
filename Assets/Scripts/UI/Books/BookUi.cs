using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class BookUi : AutoSelectFirstButtonOnEnable
{
    //made by Michaëla 2026-04-19
    //Updated by Anton 2026-05-16
    //Overhaul made by Anton 2026-05-17
    //rewriten scrambledText method and added open on lore entry by Michaëla 2026-05-30

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
    [SerializeField] private PlayerWeaponManager playerWeaponManager;
    [SerializeField] private LoreManager loreManager;
    [SerializeField] private UIManager uiManager;

    [Header("Animation")]
    [SerializeField] private RectTransform movingBook;
    [SerializeField] private Transform movingPage;

    [SerializeField] private float animationDurationMoving = 1f;
    [SerializeField] private float animationDurationOpening = 0.5f;
    [SerializeField] public bool isAnimating;
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
    private LoreEntry pendingLoreEntry;

    public enum BookTabEnum
    {
        Stats,
        Cards,
        Lore
    }

    private BookTabEnum currentTab = BookTabEnum.Stats;

    protected override void OnEnable()
    {
        base.OnEnable();

        isAnimating = true;

        SetClosedInstant();
        BaseBookSetup();

        if (pendingLoreEntry != null)
        {
            OpenLoreAtEntry(pendingLoreEntry);
            pendingLoreEntry = null;
        }

        DisableTabButtonsTemporarily();
        StartCoroutine(AnimateSize(() =>
        {
            AnimateMove(() =>
            {
                AnimateOpen(() =>
                {
                    isAnimating = false;
                    //Debug.Log($"isAnimating is: {false}");
                    EnableTabButtons();
                    UpdateTabButtons();
                });
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
    public void BaseBookSetup()
    {
        BuildStatPages(playerStats, playerWeaponManager);
        BuildCardPages();
        BuildLorePages();

        OpenTab(BookTabEnum.Stats);
        UpdateTabButtons();
    }

    public void SpecificBookSetup(BookTabEnum tab)
    {
        BuildStatPages(playerStats, playerWeaponManager);
        BuildCardPages();
        BuildLorePages();

        OpenTab(tab);
        UpdateTabButtons();
    }

    public void BuildStatPages(PlayerStats stats, PlayerWeaponManager weaponStats)
    {
        statsPages.Clear();

        statsPages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats,
            weaponStats = weaponStats
        });

        statsPages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats,
            weaponStats = weaponStats
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
                    loreTitle = entry.title,
                    loreText = unlocked ? page : ScrambleText(page)
                });
            }
        }
    }

    // Helper method for scrambling text

    private string ScrambleText(string text)
    {
        string symbols = "@#$%&";

        //Remove layout of text
        string cleanText = new string(text.Where(c => !char.IsWhiteSpace(c)).ToArray());

        char[] chars = cleanText.ToCharArray();

        //heavy scrable
        for (int i = 0; i < chars.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, chars.Length);

            (chars[i], chars[randomIndex]) = (chars[randomIndex], chars[i]);
        }

        // Corrupt some letters into symbols

        for (int i = 0; i < chars.Length; i++)
        {
            if (UnityEngine.Random.value < 0.1f)
            {
                chars[i] = (symbols[UnityEngine.Random.Range(0, symbols.Length)]);
            }
        }

        // rebuild into block text
        int lineLenght = 38;


        System.Text.StringBuilder builder =
            new System.Text.StringBuilder();

        for (int i = 0; i < chars.Length; i++)
        {
            builder.Append(chars[i]);

            // Bigger fragmented spacing
            if (UnityEngine.Random.value < 0.12f)
            {
                int extraSpaces =
                    UnityEngine.Random.Range(1, 4);

                builder.Append(new string(' ', extraSpaces));
            }

            // Normal line breaks
            if ((i + 1) % lineLenght == 0)
            {
                builder.Append("\n");

                // Sometimes add completely blank lines
                if (UnityEngine.Random.value < 0.28f)
                {
                    builder.Append("\n");
                }
            }
            
        }
        return builder.ToString();
    }

    public void ChangeTab(BookTabEnum tab)
    {
        if (isAnimating)
            return;

        isAnimating = true;

        DisableTabButtonsTemporarily();
        AnimateClose(() =>
        {
            OpenTab(tab);

            AnimateOpen(() =>
            {
                isAnimating = false;

                EnableTabButtons();
                UpdateTabButtons();
                EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
            });
        });
    }

    public void OpenTab(BookTabEnum tab, int index = 0)
    {
        currentTab = tab;
        currentIndex = index;

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
            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.bookPageFlipEvent);
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
            RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.bookPageFlipEvent);
            ShowPages();

        }
    }
    public void CloseBook()
    {
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
        ChangeTab(BookTabEnum.Lore);
    }
    public void SetPendingLoreEntry(LoreEntry entry)
    {
        pendingLoreEntry = entry;
    }
    public void OpenLoreAtEntry(LoreEntry entry)
    {
        int loreIndex = allLoreEntries.IndexOf(entry);

        if (loreIndex < 0)
            loreIndex = 0;

        currentIndex = (loreIndex / 2) * 2;

        OpenTab(BookTabEnum.Lore, currentIndex);
    }
    private void EnableTabButtons()
    {
        statsButton.interactable = true;
        cardsButton.interactable = true;
        loreButton.interactable = true;
    }

    private void DisableTabButtonsTemporarily()
    {
        statsButton.interactable = false;
        cardsButton.interactable = false;
        loreButton.interactable = false;
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

    public void AnimateMove(Action onComplete = null, bool reverse = false)
    {
        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.bookSlideEvent);

        Vector3 pos = reverse ? startPos : targetPos;

        LeanTween.moveLocal(movingBook.gameObject, pos, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void AnimateOpen(Action onComplete = null)
    {
        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.bookOpenEvent);

        LeanTween.rotateAroundLocal(movingPage.gameObject, Vector3.forward, rotationAngle, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void AnimateClose(Action onComplete = null)
    {
        RuntimeManager.PlayOneShot(WorldSoundFXManager.instance.bookCloseEvent);

        LeanTween.rotateAroundLocal(movingPage.gameObject, Vector3.forward, -rotationAngle, animationDurationOpening).setEase(LeanTweenType.easeInOutQuad).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public IEnumerator AnimateSize(Action onComplete = null, bool reverse = false)
    {
        float timer = 0f;
        float animationSpeed;
        while (timer < animationDurationMoving)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / (animationSpeed = reverse ? animationDurationMoving * 0.75f : animationDurationMoving);

            float curvevalue = reverse ? bounceCurve.Evaluate(1f - t) : bounceCurve.Evaluate(t);

            movingBook.transform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        movingBook.transform.localScale = reverse  ? Vector3.zero : Vector3.one;

        onComplete?.Invoke();
    }
}
