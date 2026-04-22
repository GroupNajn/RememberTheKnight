using UnityEngine;

public class PlayerVFX : CharacterVFX
{
    // ONLY NEEDS TO BE EMPTY FOR NOW
    // CAN BE USED FOR PLAYER SPECIFIK VFX
    [Header("Slam VFX")]
    public GameObject SlamVFX;
    public float ForwardOffset = 1f;
    // public float ForwardOffset;

    public void PlaySlamVFX()
    {
        GameObject bloodSplatter = Instantiate(SlamVFX, this.gameObject.transform.position + this.gameObject.transform.forward * ForwardOffset, Quaternion.identity);
        Destroy(bloodSplatter, 2f);
    }
}
