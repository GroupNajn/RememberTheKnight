using UnityEngine;

public class PlayerVFX : CharacterVFX
{
    [Header("Slam VFX")]
    public GameObject SlamVFX;
    public float ForwardOffset = 1f;

    [Header("Heal VFX")]
    public GameObject HealVFX;
    [Header("Arrow VFX")]
    public GameObject Arrow_VFX;

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
    public void PlayArrowVFX(Vector3 contactPoint)
    {
        GameObject root = GameObject.FindWithTag("Root");
        GameObject VFX = Instantiate(Arrow_VFX, contactPoint, Quaternion.LookRotation(contactPoint - root.transform.position) * Quaternion.Euler(0, 180, 0), root.transform);
        Destroy(VFX, 10f);
    }

    public void Update()
    {
        //if arrows in player body, take damage when rolling

        if (this.gameObject.GetComponent<PlayerStates>().CurrentMoveState == MoveState.Dodging)
        {
            foreach (Transform child in GameObject.FindWithTag("Root").GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("Arrow_VFX"))
                {
                    PlayerManager playerManager = this.gameObject.GetComponent<PlayerManager>();
                    playerManager.TakeDamage(5, child.position);
                    Destroy(child.gameObject);
                }
            }
        }
    }
}