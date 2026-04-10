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
        DontDestroyOnLoad(gameObject); // TODO: Remove, should be in a parent object
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length + 2); // Wait for the fade-out animation to complete
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene previousScene, LoadSceneMode newScene)
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
    }

    IEnumerator FadeOut()
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length + 2); // Wait for the fade-out animation to complete
    }
}