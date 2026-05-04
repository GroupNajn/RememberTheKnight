using UnityEngine;
using System.Collections.Generic;
public class BookUi : MonoBehaviour
{
    //made by Michaëla 2026-04-19

    // Todo - make when pressing tab buttons keep pages on same page as now back does not work if pressed tab if pages stats has only one page.
    [Header("Pages")]
    [SerializeField] private BookPageUI leftPage;
    [SerializeField] private BookPageUI rightPage;

    [Header("Text Book Input")]
    private string bookText;
    [SerializeField] private int charsPerPage = 300;

    [SerializeField] private UnityEngine.UI.Button statsButton;
    [SerializeField] private UnityEngine.UI.Button cardsButton;
    [SerializeField] private UnityEngine.UI.Button loreButton;

    private List<PageData> pages = new List<PageData>();
    [SerializeField] private List<LoreEntry> allLoreEntries;
    private HashSet<string> unlockedLore = new HashSet<string>();
    [SerializeField] private CardSelectionUI cardSelectionUI;

    private int currentIndex = 0;
    private int statsPageIndex = -1;
    private int cardsPageIndex = -1;
    private int lorePageIndex = -1;


    //public void SetBookText(string text)
    //{
    //    bookText = text;
    //}


    // Build inventory book
    public void BuildInventory(PlayerStats stats)
    {
        pages.Clear();
        List<CardData> allCards = new List<CardData>();

        statsPageIndex = pages.Count;
        // First page = stats
        pages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats
        });
        cardsPageIndex = pages.Count;

        // Cards (4 per page)

        if (cardSelectionUI != null && cardSelectionUI.selectedCardData != null)
        {
            allCards.AddRange(cardSelectionUI.selectedCardData);
        }
        if (allCards != null && allCards.Count > 0)
        {
            cardsPageIndex = pages.Count;

            for (int i = 0; i < allCards.Count; i += 4)
            {
                pages.Add(new PageData
                {
                    type = PageData.PageType.Cards,
                    cards = allCards.GetRange(i, Mathf.Min(4, allCards.Count - i))
                });
            }
        }
        else
        {
            cardsPageIndex = -1;
        }

        // Lore
        UnlockLore("1"); // For testing, unlock the first lore entry. In a real game, this would be based on player actions.
        if (unlockedLore.Count > 0)
        {
            lorePageIndex = pages.Count;

            foreach (var entry in allLoreEntries)
            {
                if (!unlockedLore.Contains(entry.id))
                    continue;

                var entryPages = entry.GetPages(charsPerPage);

                foreach (var page in entryPages)
                {
                    pages.Add(new PageData
                    {
                        type = PageData.PageType.Text,
                        text = page
                    });
                }
            }
        }
        else
        {
            lorePageIndex = -1;
        }
            currentIndex = 0;
        ShowPages();
        UpdateTabButtons();
        Debug.Log("BuildInventory called. Pages: " + pages.Count);
    }

    //public void BuildInventory(List<CardData> allCards, PlayerStats stats)
    //{
    //    pages.Clear();

    //    statsPageIndex = pages.Count;
    //    // First page = stats
    //    pages.Add(new PageData
    //    {
    //        type = PageData.PageType.Stats,
    //        stats = stats
    //    });
    //    cardsPageIndex = pages.Count;

    //    // Cards (4 per page)
    //    if (allCards != null && allCards.Count > 0)
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
    //    UnlockLore("1");
    //    if (unlockedLore.Count > 0)
    //    {
    //        lorePageIndex = pages.Count;

    //        foreach (var entry in allLoreEntries)
    //        {
    //            if (!unlockedLore.Contains(entry.id))
    //                continue;

    //            var entryPages = entry.GetPages(charsPerPage);

    //            foreach (var page in entryPages)
    //            {
    //                pages.Add(new PageData
    //                {
    //                    type = PageData.PageType.Text,
    //                    text = page
    //                });
    //            }
    //        }
    //    }
    //    else
    //    {
    //        lorePageIndex = -1;
    //    }
    //    currentIndex = 0;
    //    ShowPages();
    //    UpdateTabButtons();
    //    Debug.Log("BuildInventory called. Pages: " + pages.Count);
    //}


    // Display current pages
    public void ShowPages()
    {
        // left 
        if (currentIndex < pages.Count)
        {
            leftPage.gameObject.SetActive(true);
            leftPage.Setup(pages[currentIndex]);
        }
        else
        {
            leftPage.gameObject.SetActive(false);
        }

        // Right
        if (currentIndex + 1 < pages.Count)
        {
            rightPage.gameObject.SetActive(true);
            rightPage.Setup(pages[currentIndex + 1]);
        }
        else
        {
            rightPage.gameObject.SetActive(false);
        }
        Debug.Log($"Index: {currentIndex}, Total Pages: {pages.Count}");
    }

    // Flip forward
    public void NextPage()
    {
        if (currentIndex + 2 < pages.Count)
        {
            currentIndex += 2;
            ShowPages();
        }
    }

    // Flip backward
    public void PrevPage()
    {
        if (currentIndex - 2 >= 0)
        {
            currentIndex -= 2;
            ShowPages();
        }
    }

    //private List<string> SplitTextWords(string text, int maxChars)
    //{
    //    List<string> pages = new List<string>();
    //    string[] words = text.Split(' ');

    //    string current = "";

    //    foreach (var word in words)
    //    {
    //        if ((current + word).Length > maxChars)
    //        {
    //            pages.Add(current);
    //            current = "";
    //        }

    //        current += word + " ";
    //    }

    //    if (!string.IsNullOrWhiteSpace(current))
    //        pages.Add(current);

    //    return pages;
    //}

    // Tab buttons
    // go to the first page of the respective section, if it exists. If not, do nothing (or show a message)
    public void GoToStats()
    {
        if (statsPageIndex >= 0 && statsPageIndex < pages.Count)
        {
            currentIndex = statsPageIndex;
            ShowPages();
        }
    }

    public void GoToCards()
    {
        if (cardsPageIndex >= 0 && cardsPageIndex < pages.Count)
        {
            currentIndex = cardsPageIndex;
            ShowPages();
        }
        else
        {
            //Debug.Log("No card pages exist");
        }
    }

    public void GoToLore()
    {
        if (lorePageIndex >= 0 && lorePageIndex < pages.Count)
        {
            currentIndex = lorePageIndex;
            ShowPages();
        }
        else
        {
           // Debug.Log("No lore pages exist");
        }
    }

    // Enable or disable tab buttons based on whether their respective pages exist
    private void UpdateTabButtons()
    {
        statsButton.interactable = statsPageIndex != -1;
        cardsButton.interactable = cardsPageIndex != -1;
        loreButton.interactable = lorePageIndex != -1;
    }

    //unlcok lore by id, if not already unlocked. In a real game, this would be called when the player discovers new lore.
    public void UnlockLore(string id)
    {
        if (unlockedLore.Contains(id))
            return;

        unlockedLore.Add(id);
        //Debug.Log("Unlocked lore: " + id);
    }
}
