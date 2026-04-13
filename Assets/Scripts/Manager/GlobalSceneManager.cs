using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    [SerializeField] Animator transitionAnimator;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        transitionAnimator.ResetTrigger("FadeFromBlack");
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null; // Wait a frame to let the animator switch state
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
    }

    IEnumerator FadeOut()
    {
        //transitionAnimator.SetTrigger("FadeFromBlack");
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }
}