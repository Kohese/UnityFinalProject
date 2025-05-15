using UnityEngine;
using Unity.Netcode;
using WeaponStrategyPattern;

public class PlayerWeaponInput : NetworkBehaviour
{
    private WeaponTiers weaponTiers;
    private PlayerWeaponTierTracker weaponIdx;
    private GameObject player;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (IsOwner)
        {
            player = gameObject;
        }
        weaponTiers = GetComponent<WeaponTiers>();
        weaponIdx = GetComponent<PlayerWeaponTierTracker>();
        Debug.Log($"This is current: {weaponIdx.currentWeaponIndex.Value}");
        weaponIdx.currentWeaponIndex.OnValueChanged += (_, newVal) => { Debug.Log($"This is what is being changed to {newVal}"); weaponTiers.SwitchWeapon(newVal);};
    }

    void Update()
    {
        // if (!IsOwner) return;
        // Debug.Log($"Current kill count: {weaponIdx.killCount.Value}");
        

        // // right now just switch guns using 1-5 keys
        // if (Input.GetKeyDown(KeyCode.Alpha1)) weaponTiers.SwitchWeapon(1);
        // if (Input.GetKeyDown(KeyCode.Alpha2)) weaponTiers.SwitchWeapon(2);
        // if (Input.GetKeyDown(KeyCode.Alpha3)) weaponTiers.SwitchWeapon(3);
        // if (Input.GetKeyDown(KeyCode.Alpha4)) weaponTiers.SwitchWeapon(4);
        // if (Input.GetKeyDown(KeyCode.Alpha5)) weaponTiers.SwitchWeapon(5);
    }
}
