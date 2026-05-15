using System.Collections.Generic;
using UnityEngine;

public class ReadableBookUI : MonoBehaviour
{
    [SerializeField] private ReadablePageUI leftPage;
    [SerializeField] private ReadablePageUI rightPage;

    [SerializeField] private int charsPerPage = 300;

    private List<string> pages = new List<string>();

    private int currentIndex = 0;

    public void OpenBooke(LoreEntry entry)
    {        
        pages = entry.GetPages(charsPerPage);
        currentIndex = 0;
        ShowPages();
    }

    private void ShowPages()
    {
        // left 
        if (currentIndex < pages.Count)
        {
            leftPage.gameObject.SetActive(true);
            leftPage.SetText(pages[currentIndex]);
        }
        else
        {
            leftPage.gameObject.SetActive(false);
        }

        // Right
        if (currentIndex + 1 < pages.Count)
        {
            rightPage.gameObject.SetActive(true);
            rightPage.SetText(pages[currentIndex + 1]);
        }
        else
        {
            rightPage.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (currentIndex + 2 < pages.Count)
        {
            currentIndex += 2;
            ShowPages();
        }
    }
    public void PreviousPage() 
    {
        if (currentIndex - 2 >= 0)
        {
            currentIndex -= 2;
            ShowPages();
        }
    }
}