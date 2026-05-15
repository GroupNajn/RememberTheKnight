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
    [SerializeField] private List<LoreEntry> allLoreEntries = new();
    [SerializeField] private PlayerCollection playerCollection;
    [SerializeField] private LoreManager loreManager;

    private int currentIndex = 0;
    private int statsPageIndex = -1;
    private int cardsPageIndex = -1;
    private int lorePageIndex = -1;

    public void BuildInventory(PlayerStats stats)
    {
        pages.Clear();

        List<CardData> allCards = new List<CardData>();

        // Get cards from player collection
        allCards.AddRange(playerCollection.ReturnPermanentCardCollection());
        allCards.AddRange(playerCollection.ReturnTempCardCollection());

        statsPageIndex = pages.Count;

        pages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats
        });

        // Cards
        if (allCards.Count > 0)
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
        loreManager.UnlockLore("1");
        
        lorePageIndex = pages.Count;

        foreach (var entry in allLoreEntries) 
        {
            //loreManager.UnlockLore(entry.id);
            bool unlocked = loreManager.IsLoreUnlocked(entry.id);

            var entryPages = entry.GetPages(charsPerPage);

            if (unlocked)
            {
                foreach (var page in entryPages)
                {
                    pages.Add(new PageData
                    {
                        type = PageData.PageType.Text,
                        text = page
                    });
                }
            }
            else
            {
                foreach (var _ in entryPages)
                {
                    pages.Add(new PageData
                    {
                        type = PageData.PageType.Text,
                        text = "???"
                    });
                }
            }
        }
        currentIndex = 0;
        ShowPages();
        UpdateTabButtons();
    }


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
    
}
