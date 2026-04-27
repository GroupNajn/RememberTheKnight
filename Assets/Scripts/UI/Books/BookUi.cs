using UnityEngine;
using System.Collections.Generic;
public class BookUi : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private BookPageUI leftPage;
    [SerializeField] private BookPageUI rightPage;

    [Header("Text Book Input")]
    [SerializeField] private string bookText;
    [SerializeField] private int charsPerPage = 300;

    private List<PageData> pages = new List<PageData>();
    private int currentIndex = 0;

    // Build inventory book
    public void BuildInventory(List<CardData> allCards, PlayerStats stats)
    {
        pages.Clear();

        // First page = stats
        pages.Add(new PageData
        {
            type = PageData.PageType.Stats,
            stats = stats
        });

        // Cards (4 per page)
        for (int i = 0; i < allCards.Count; i += 4)
        {
            pages.Add(new PageData
            {
                type = PageData.PageType.Cards,
                cards = allCards.GetRange(i, Mathf.Min(4, allCards.Count - i))
            });
        }

        if (!string.IsNullOrEmpty(bookText))
        {
            var chunks = SplitTextWords(bookText, 300); // adjust size

            foreach (var chunk in chunks)
            {
                pages.Add(new PageData
                {
                    type = PageData.PageType.Text,
                    text = chunk
                });
            }
        }

        currentIndex = 0;
        ShowPages();
    }

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

    private List<string> SplitTextWords(string text, int maxChars)
    {
        List<string> pages = new List<string>();
        string[] words = text.Split(' ');

        string current = "";

        foreach (var word in words)
        {
            if ((current + word).Length > maxChars)
            {
                pages.Add(current);
                current = "";
            }

            current += word + " ";
        }

        if (!string.IsNullOrWhiteSpace(current))
            pages.Add(current);

        return pages;
    }
}
