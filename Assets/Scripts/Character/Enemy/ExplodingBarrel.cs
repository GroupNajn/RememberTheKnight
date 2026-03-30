using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    // Made by Lukas 2026-03-20
    ExplotionTest explotionTest;
    void Awake()
    {
        explotionTest = GetComponent<ExplotionTest>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Explode();
        }
    }

    public void Explode()
    {
        explotionTest.Explode();
    }
}