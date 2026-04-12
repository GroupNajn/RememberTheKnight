using UnityEngine;

public class OptionMenuUI : MonoBehaviour
{
    UIManager uiManager;
    //SceneData sceneData;
    void Awake()
    {
    }
    private void Start()
    {
        uiManager = GetComponentInParent<UIManager>();
    }
    public void OpenControlls()
    {
        // NÄR VI LÄGGER TILL KONTROLL INSTÄLLNINGAR/
    }

    public void OpenAudio()
    {
        // NÄR VI LÄGGER TILL AUDIO INSTÄLLNINGAR
    }

    public void OpenVideo()
    {
        // NÄR VI LÄGGER TILL VIDEO INSTÄLLNINGAR
    }

    public void BackToStartMenu()
    {
        uiManager.OpenStartMenu();
        uiManager.CloseStartOptionMenu();
    }
}
