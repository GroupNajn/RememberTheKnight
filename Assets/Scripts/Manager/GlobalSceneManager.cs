using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    Animator transitionAnimator;
    bool useTransition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        transitionAnimator = GameObject.FindGameObjectWithTag("BlackFade").GetComponent<Animator>();

        transitionAnimator.ResetTrigger("FadeFromBlack");
    }

    public void LoadSceneNoTransition(string sceneName)
    {
        useTransition = false;
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    public void LoadSceneTransition(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        useTransition = true;
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null; // Wait a frame to let the animator switch state
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (transitionAnimator)
        {
            if (useTransition)
            {
                StartCoroutine(FadeOut());
            }
        }
        else
        {
            Debug.Log("transitionAnimator is null, if this is not when starting the game, this is a problem");
        }
    }

    IEnumerator FadeOut()
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        // Add event to trigger when the fade-out animation is complete, if needed
    }
}