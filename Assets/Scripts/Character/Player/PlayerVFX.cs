using UnityEngine;

public class PlayerVFX : CharacterVFX
{
    // ONLY NEEDS TO BE EMPTY FOR NOW
    // CAN BE USED FOR PLAYER SPECIFIK VFX
    [Header("Slam VFX")]
    public GameObject SlamVFX;
    public float ForwardOffset = 1f;

    [Header("Heal VFX")]
    public GameObject HealVFX;
    // public float ForwardOffset;

    public void PlaySlamVFX()
    {
        GameObject VFX = Instantiate(SlamVFX, this.gameObject.transform.position + this.gameObject.transform.forward * ForwardOffset, Quaternion.identity);
        Destroy(VFX, 2f);
    }
    public void PlayHealVFX()
    {
        GameObject VFX = Instantiate(HealVFX, this.gameObject.transform.position + this.gameObject.transform.forward * ForwardOffset, Quaternion.identity);
        Destroy(VFX, 3f);
    }
}
