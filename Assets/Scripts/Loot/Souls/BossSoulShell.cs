using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class BossSoulShell : MonoBehaviour
{
    [SerializeField] VisualEffect VFX;

    private bool floatIncrease = true;
    private float shellFloat;

    private float min = 0.5f;
    private float max = 1f;
    private float speed = 1f;

    void Start()
    {
        shellFloat = VFX.GetFloat("ShellTransformer");
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        shellFloat = Mathf.Lerp(min, max, t);

        VFX.SetFloat("ShellTransformer", shellFloat);
    }

    private float LoopFloatValue()
    {
        if (floatIncrease)
            shellFloat += speed * Time.deltaTime;
        else
            shellFloat -= speed * Time.deltaTime;

        if (shellFloat >= max)
            floatIncrease = false;

        if (shellFloat <= min)
            floatIncrease = true;

        return shellFloat;
    }
}
