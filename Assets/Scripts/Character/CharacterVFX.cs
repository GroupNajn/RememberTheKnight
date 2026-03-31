using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    // Made by Wilmer 2026-03-28
    [SerializeField] GameObject bloodSplatterVFX;

    private void Start()
    {

    }
    public void PlayBloodSplatter(Vector3 contactPoint)
    {
        GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        Destroy(bloodSplatter, 5f);
    }
}
