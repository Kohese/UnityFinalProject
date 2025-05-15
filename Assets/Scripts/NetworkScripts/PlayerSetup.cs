using UnityEngine;
using Unity.Netcode;

public class PlayerSetup : NetworkBehaviour
{
    [Header("Skin Setup")]
    [SerializeField] private Transform skinsParent; // The "Skins" object holding all skins

    private int currentSkinIndex;
    // private GameObject DataSource;

    public void InitWithSkin(int skinIndex)
    {
        currentSkinIndex = Random.Range(0, 7);
        if (skinsParent == null)
        {
            Debug.LogError("Skins parent not assigned!");
            return;
        }

        currentSkinIndex = Mathf.Clamp(skinIndex, 0, skinsParent.childCount - 1);

        // Deactivate all skins
        for (int i = 0; i < skinsParent.childCount; i++)
        {
            skinsParent.GetChild(i).gameObject.SetActive(i == currentSkinIndex);
        }

        Debug.Log($"[PlayerSetup] Applied skin index {currentSkinIndex} for client {OwnerClientId}");
    }

    public int GetCurrentSkin()
    {
        return currentSkinIndex;
    }
}
