using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    PlayerPrefs playerPrefs;

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
