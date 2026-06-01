using UnityEngine;

public static class PlayerPrefsSaveSystem
{
    /// <summary>
    /// Created by Anton 2026-05-06
    /// Originally made a save system for interactable objects
    /// This was to save the state of interactable objects, such as if they have been interacted with or not, and if they have been unlocked or not.
    /// 
    /// Change by Anton and Henric 2026-05-28
    /// Changed the save system to a general system for playerprefs to save data locally
    /// </summary>


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

    public static bool HasUnlockedCard(string cardID)
    {
        return PlayerPrefs.GetInt(cardID, 0) == 1;
    }

    // Returns true if the string id has been killed.
    public static bool HasKilledBoss(string bossID)
    {
        return PlayerPrefs.GetInt(bossID, 0) == 1;
    }

    public static bool HasPlayedGame()
    {
        return PlayerPrefs.GetInt("HasPlayedGame", 0) == 1;
    }

    public static void SaveBossKill(string bossID)
    {
        PlayerPrefs.SetInt(bossID, 1);
    }

    public static void SaveCharactedCustomization(string bodyPartID, int index)
    {
        PlayerPrefs.SetInt(bodyPartID, index);
    }

    public static void SaveGender(SwitchBodyParts.Gender gender)
    {
        PlayerPrefs.SetInt("Gender", (int)gender);
    }

    public static int GetSavedGender()
    {
        return PlayerPrefs.GetInt("Gender", 0);
    }

    public static int GetSavedCharacterCustomization(string bodyPartID)
    {
        return PlayerPrefs.GetInt(bodyPartID);
    }

    public static void SaveSouls(string soulValueText,int amount)
    {
        PlayerPrefs.SetInt(soulValueText, amount);
    }

    public static void SaveSoulsDonatedToFamily(int amount)
    {
        //PlayerPrefs.
    }
    public static int GetSavedSouls(string soulValueText)
    {
        int souls = PlayerPrefs.GetInt(soulValueText);
        if (souls <= 0) return 4;
        return souls;
    }

    public static int GetSavedSoulsDonatedSinecLast(string soulValueText)
    {
        return PlayerPrefs.GetInt(soulValueText);

    }


    // Saves the int value in playerprefs locally in order to keep progress.
    public static void SetSaveState(string ID)
    {
        PlayerPrefs.SetInt(ID, 1);
        PlayerPrefs.Save();
    }
}
