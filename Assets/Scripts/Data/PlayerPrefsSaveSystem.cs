using UnityEngine;

public static class PlayerPrefsSaveSystem
{
    // Created by Anton
    // Made a save system for playerprefs to save data locally


    // Returns true if the string id has been unlocked.
    public static bool HasUnlocked(string unlockedID)
    {
        return PlayerPrefs.GetInt(unlockedID, 0) == 1;
    }


    // Returns true if the string id has been interacted with
    public static bool HasInteracted(string interactableID)
    {
        return PlayerPrefs.GetInt(interactableID, 0) == 1;
    }


    // Saves the int value in playerprefs locally in order to keep progress.
    public static void SetSaveState(string ID)
    {
        PlayerPrefs.SetInt(ID, 1);
        PlayerPrefs.Save();
    }
}
