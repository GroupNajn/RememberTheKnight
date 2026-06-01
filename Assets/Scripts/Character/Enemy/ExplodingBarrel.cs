using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-03-20
    /// A simple method to play the explotion
    /// </summary>

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