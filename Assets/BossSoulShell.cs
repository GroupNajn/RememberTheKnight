using UnityEngine;
using UnityEngine.VFX;

public class BossSoulShell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] VisualEffect VFX;
    private bool floatIncrease;
    private float shellFloat;
    void Start()
    {
        shellFloat = VFX.GetFloat("ShellTransformer");
    }

    // Update is called once per frame
    void Update()
    {
        VFX.SetFloat("ShellTransformer", LoopFloatValue());
            shellFloat = VFX.GetFloat("ShellTransformer");
    }

    private float LoopFloatValue()
    {
        if (!AlternateFloatCheck())
            shellFloat--;
        shellFloat++;
        return shellFloat;

    }

    private bool AlternateFloatCheck()
    {
        
        if(shellFloat>= 0.5f)
            floatIncrease = true;
        else if(shellFloat>=1f)
            floatIncrease = false;
        return floatIncrease;
    }

}
