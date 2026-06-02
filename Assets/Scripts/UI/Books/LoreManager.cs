using UnityEngine;
using System.Collections.Generic;

public class LoreManager : MonoBehaviour
{
    private HashSet<string> unlockedLore = new HashSet<string>();
    [SerializeField] private List<LoreEntry> allLoreEntries;

    private void Start()
    {
        foreach (var lore in allLoreEntries)
        {
            if (PlayerPrefsSaveSystem.HasInteracted(lore.id))
            {
                unlockedLore.Add(lore.id);
            }
        }
    }

    public void UnlockLore(string id)
    {
        unlockedLore.Add(id);
    }
    
    public bool IsLoreUnlocked(string id)
    {
        return unlockedLore.Contains(id);
    }

    public HashSet<string> GetUnlockedLore()
    {   
        return unlockedLore;
    }
}
