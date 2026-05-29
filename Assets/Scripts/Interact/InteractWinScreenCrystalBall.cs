using System.Collections;
using UnityEngine;

public class InteractWinScreenCrystalBall : MonoBehaviour, IInteractable, IInteractableUIText
{
    public void Interact()
    {
        UIManager.Instance.UIMenuActive = true;
        StartCoroutine(WinDelay());
    }

    IEnumerator WinDelay()
    {
        GameObject blackFade = GameObject.FindGameObjectWithTag("BlackFade");

        Animator blackFadeAnimator = blackFade.GetComponent<Animator>();

        Transform[] blackFadeChildren = blackFade.GetComponentsInChildren<Transform>(true);
        int count = blackFadeChildren.Length;

        for (int i = count - 1; i > 0; i--)
        {
            blackFadeChildren[i].gameObject.SetActive(false);
        }

        blackFadeAnimator.SetTrigger("FadeToBlack");
        yield return null;
        yield return new WaitForSecondsRealtime(blackFadeAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        Event_System.instance.OnWin?.Invoke();
        blackFadeAnimator.SetTrigger("FadeFromBlack");
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "The End...?";
        return UIData;
    }
}