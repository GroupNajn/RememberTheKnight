using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // OM SPELAREN VI SKA BEHÅLLA SPELARE MELLAN SCENE SÅ SKAAAA DEN SPARAS HÄR
    }
}