using UnityEngine;

public static class InteractableSaveSystem
{
    public static bool HasInteracted(string interactableID)
    {
        return PlayerPrefs.GetInt(interactableID, 0) == 1;
    }

    public static void SetInteracted(string interactableID)
    {
        PlayerPrefs.SetInt(interactableID, 1);
        PlayerPrefs.Save();
    }
}
