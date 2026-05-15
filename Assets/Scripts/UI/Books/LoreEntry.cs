using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Lore Entry")]
public class LoreEntry : ScriptableObject
{
    // Made by Michaëla 2026-04-30
    public string id;
    public string title;
    [TextArea(10, 30)]
    public string fullText;

    // This generates pages from the full text
    public List<string> GetPages(int maxCharsPerPage)
    {
        List<string> pages = new List<string>();

        if (string.IsNullOrEmpty(fullText))
            return pages;

        string[] words = fullText.Split(' ');
        string current = "";

        foreach (var word in words)
        {
            if ((current + word).Length > maxCharsPerPage)
            {
                if (!string.IsNullOrWhiteSpace(current))
                    pages.Add(current.Trim());

                current = "";
            }

            current += word + " ";
        }

        if (!string.IsNullOrWhiteSpace(current))
            pages.Add(current.Trim());

        return pages;
    }
}
