
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevPageUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private RectTransform devPageWindowRect;
    [SerializeField] private float duration = 1f;
    [SerializeField] AnimationCurve bounceCurve;
    [SerializeField] List<GameObject> gameObjectsToDisable;
    [SerializeField] TextMeshProUGUI devTextTMP;

    [Header("References")]
    [SerializeField] private UIManager uiManager;

    private void OnEnable()
    {
        DisableGameObjects();
        StartCoroutine(ScaleBouncePickUpWindow());
    }

    private IEnumerator ScaleBouncePickUpWindow()
    {
        float timer = 0f;
        // 300 x 450
        // 300 x 1.5
        // 350 x 525
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / duration;
            float curvevalue = bounceCurve.Evaluate(t);

            devPageWindowRect.transform.localScale = Vector3.one * curvevalue;

            yield return null;
        }
        devPageWindowRect.transform.localScale = Vector3.one;
        //LeanTween.scale(rescaleRect, new Vector3(0.95f, 0.95f, 0.95f), 1.5f).setEaseInBack().setLoopPingPong().setIgnoreTimeScale(true);
        EnableGameObjects();
    }


    public void OnClose()
    {
        uiManager.CloseDevSecretUI();
    }

    void EnableGameObjects()
    {
        foreach (GameObject go in gameObjectsToDisable)
        {
            go.SetActive(true);
        }
    }

    void DisableGameObjects()
    {

        foreach (GameObject go in gameObjectsToDisable)
        {
            go.SetActive(false);
        }

    }
}
