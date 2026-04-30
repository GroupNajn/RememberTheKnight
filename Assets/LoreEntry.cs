using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LoreEntry 
{
    // Made by Michaëla 2026-04-30
    public string id;

    [TextArea(10, 30)]
    public string fullText;

    // This generates pages from the full text
    public List<string> GetPages(int maxCharsPerPage)
    {
        List<string> pages = new List<string>();

        if (string.IsNullOrEmpty(fullText))
            return pages;

        // Split the full text into words to avoid breaking words across pages
        string[] words = fullText.Split(' ');
        string current = "";

        foreach (var word in words)
        {
            if ((current + word).Length > maxCharsPerPage)
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
