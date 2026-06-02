using System.Diagnostics;
using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    PlayerPrefs playerPrefs;
    [SerializeField] GameObject warningUI;

    public void OpenWarning()
    {
        warningUI.SetActive(true);
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        string exePath = Process.GetCurrentProcess().MainModule.FileName;
        Process.Start(exePath);
        Application.Quit();

    }

    public void CloseWarning()
    {
        warningUI.SetActive(false);
    }


}
