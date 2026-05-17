using UnityEngine;

public class LeanTweenResetter : MonoBehaviour
{
    void Start()
    {
        
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetLeanTween()
    {
        LeanTween.reset();
    }

    void Update()
    {
        
    }
}
