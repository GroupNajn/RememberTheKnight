using UnityEngine;
using System.Collections.Generic;

public class LoreManager : MonoBehaviour
{
    private HashSet<string> unlockedLore = new HashSet<string>();

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
