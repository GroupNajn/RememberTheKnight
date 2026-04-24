using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    // Made by Lukas 2026-03-20
    Explosion explosion;
    void Awake()
    {
        explosion = GetComponent<Explosion>();
    }

    public void Explode()
    {
        explosion.OnExplode();
    }
}