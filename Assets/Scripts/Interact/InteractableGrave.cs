using UnityEngine;

public class InteractableGrave : MonoBehaviour, IInteractable, IInteractableUIText
{
    private PlayerWeaponManager playerWeaponManager;

    [Header("Grave Info")]
    [SerializeField] private ParticleSystem dirtEffect;
    [SerializeField] private GameObject soulSpawnPos;
    [SerializeField] private GameObject enemySpawnPos;
    [SerializeField] private GameObject graveSkeleton;
    [SerializeField] private GameObject graveSoul;

    private void Start()
    {
        playerWeaponManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeaponManager>();
    }

    public void Interact()
    {
        if (playerWeaponManager.currentActiveWeaponData.WeaponName == "Shovel")
        {
            LootManager.instance.SpawnSoul(soulSpawnPos.transform.position);
        }
    }

    public InteractableUIData GetUIData()
    {
        if (playerWeaponManager.currentActiveWeaponData.WeaponName == "Shovel")
        {
            var UIData = new InteractableUIData();
            UIData.CanInteract = true;

            UIData.InfoText = "Excavate";
            return UIData;
        }

        return null;
    }
}
