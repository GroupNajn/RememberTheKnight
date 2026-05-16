using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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

    [Header("Data")]
    [SerializeField] private List<LoreEntry> allLoreEntries = new();
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LoreManager loreManager;
    [SerializeField] private UIManager uiManager;

    private List<PageData> statsPages = new();
    private List<PageData> cardPages = new();
    private List<PageData> lorePages = new();

    private List<PageData> currentPages = new();

    private int currentIndex = 0;

    public enum BookTab
    {
        Stats,
        Cards,
        Lore
    }

    private BookTab currentTab = BookTab.Stats;

    public void OnEnable()
    {
       BookSetup(playerStats);
    }

    public void BookSetup(PlayerStats stats)
    {
        BuildStatPages(stats);
        BuildCardPages();
        BuildLorePages();

        OpenTab(BookTab.Stats);
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

    public void OpenTab(BookTab tab)
    {
        currentTab = tab;
        currentIndex = 0;

        switch (tab)
        {
            case BookTab.Stats:
                currentPages = statsPages;
                break;
            case BookTab.Cards:
                currentPages = cardPages;
                break;
            case BookTab.Lore:
                currentPages = lorePages;
                break;
        }

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
        uiManager.CloseBookUI();
    }

    // Tab buttons
    // go to the first page of the respective section, if it exists. If not, do nothing (or show a message)
    public void GoToStats()
    {
        OpenTab(BookTab.Stats);
    }

    public void GoToCards()
    {
        if (cardPages.Count > 0)
            OpenTab(BookTab.Cards);
    }

    public void GoToLore()
    {
        if (lorePages.Count > 0)
            OpenTab(BookTab.Lore);
        
    }

    // Enable or disable tab buttons based on whether their respective pages exist
    private void UpdateTabButtons()
    {
        statsButton.interactable = statsPages.Count > 0;
        cardsButton.interactable = cardPages.Count > 0;
        loreButton.interactable = lorePages.Count > 0;
    }

    //unlcok lore by id, if not already unlocked. In a real game, this would be called when the player discovers new lore.

    //public void BuildInventory(PlayerStats stats)
    //{
    //    pages.Clear();

    //    List<CardData> allCards = new List<CardData>();

    //    // Get cards from player collection
    //    allCards.AddRange(playerCollection.ReturnPermanentCardCollection());
    //    allCards.AddRange(playerCollection.ReturnTempCardCollection());

    //    statsPageIndex = pages.Count;

    //    pages.Add(new PageData
    //    {
    //        type = PageData.PageType.Stats,
    //        stats = stats
    //    });

    //    // Cards
    //    if (allCards.Count > 0)
    //    {
    //        cardsPageIndex = pages.Count;

    //        for (int i = 0; i < allCards.Count; i += 4)
    //        {
    //            pages.Add(new PageData
    //            {
    //                type = PageData.PageType.Cards,
    //                cards = allCards.GetRange(i, Mathf.Min(4, allCards.Count - i))
    //            });
    //        }
    //    }
    //    else
    //    {
    //        cardsPageIndex = -1;
    //    }

    //    // Lore
    //    loreManager.UnlockLore("1");
        
    //    lorePageIndex = pages.Count;

    //    foreach (var entry in allLoreEntries) 
    //    {
    //        //loreManager.UnlockLore(entry.id);
    //        bool unlocked = loreManager.IsLoreUnlocked(entry.id);

    //        var entryPages = entry.GetPages(charsPerPage);

    //        if (unlocked)
    //        {
    //            foreach (var page in entryPages)
    //            {
    //                pages.Add(new PageData
    //                {
    //                    type = PageData.PageType.Lore,
    //                    loreText = page
    //                });
    //            }
    //        }
    //        else
    //        {
    //            foreach (var _ in entryPages)
    //            {
    //                pages.Add(new PageData
    //                {
    //                    type = PageData.PageType.Lore,
    //                    loreText = "???"
    //                });
    //            }
    //        }
    //    }
    //    currentIndex = 0;
    //    ShowPages();
    //    UpdateTabButtons();
    //}

}
