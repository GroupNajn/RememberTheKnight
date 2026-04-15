using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonUI : MonoBehaviour
{
    public void GoBack()
    {
        if (UIManager.Instance.controllsUI.activeSelf)
        {
            UIManager.Instance.GoBackFromControlls();
            return;

        }

        if (UIManager.Instance.audioUI.activeSelf)
        {
            UIManager.Instance.GoBackFromAudio();
            return;
        }

        if (UIManager.Instance.videoUI.activeSelf)
        {
            UIManager.Instance.GoBackFromVideo();
            return;
        }

        UIManager.Instance.GoBackFromOptions();
    }

}
