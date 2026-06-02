using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DamageInfo
{
    public bool IsCrit { get; private set; } = false;
    public bool IsSneak { get; set; } = false;
    public float DamageAmount { get; private set; } = 0f;
    public DamageInfo(float damageAmount = 0, bool isCrit = false)
    {
        this.DamageAmount = damageAmount;
        this.IsCrit = isCrit;

    }

    public void SetDamageAmount(float damageAmount) => this.DamageAmount = (float)damageAmount;
    public void SetIsCrit(bool value) => this.IsCrit = value;

}
