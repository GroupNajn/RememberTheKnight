using System.Threading;
using UnityEngine;

public class OldLadyAnimationScript : MonoBehaviour
{
    [SerializeField] private Animator oldLadyAnimator;

    [Header("Animation Settings")]
    [SerializeField] private bool isSitting;
    [SerializeField] private float checkInterval = 5f;
    [SerializeField] private float animationChance = 20f;

    private string[] randomStandingAnimations =
    {
        "ArmGesture",
        "ArmGestureMirror",
        "WeightShift",
        "HeadShake"
    };

    private float timer;

    private void Start()
    {
        oldLadyAnimator.Play(isSitting ? "Sitting" : "ShopKeeperIdle");

        timer = checkInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = checkInterval;

            if (isSitting)
            {
                if (Random.Range(0f, 100f) < animationChance)
                {
                    oldLadyAnimator.SetTrigger("SittingHeadShake");
                }
            }
            else
            {
                if (Random.Range(0f, 100f) < animationChance)
                {
                    PlayRandomStandingAnimation();
                }
            }
        }
    }

    private void PlayRandomStandingAnimation() 
    {
        int randomIndex = Random.Range(0, randomStandingAnimations.Length);

        string animationTrigger = randomStandingAnimations[randomIndex];
        oldLadyAnimator.SetTrigger(animationTrigger);
    }
}
