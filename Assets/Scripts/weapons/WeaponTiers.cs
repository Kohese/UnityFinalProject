using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;



namespace WeaponStrategyPattern
{
    public class WeaponTiers : NetworkBehaviour
    {
// //         private WeaponBase weapon;

        

// //         public void Start()
// //         {
// //             current = gameObject.AddComponent<WeaponPistol>();
// //             current.Initalize();
// //             Debug.Log(current);
// //         }


// //         // public void switchWeapon(int currentWeapon) // add component to camera to use weapon
// //         // {            
// //         //          if (currentWeapon != null)
// //         // {
// //         //     // current.unequip?.Invoke(); // Optional if defined
// //         //     Destroy(current);
// //         //     current = null;
// //         // }

// //         //     switch (currentWeapon) 
// //         //     {
// //         //     case 1:
// //         //         gameObject.AddComponent<WeaponPistol>();
// //         //         break;
// //         //     case 2:
// //         //         // gameObject.AddComponent<WeaponShotgun>();
// //         //         break;
// //         //     case 3:
// //         //         gameObject.AddComponent<WeaponSMG>();
// //         //         break;
// //         //     case 4:
// //         //         gameObject.AddComponent<WeaponAssaultRifle>();
// //         //         break;
// //         //     case 5:
// //         //         gameObject.AddComponent<WeaponSniper>();
// //         //         break;
// //         //     default:    // Default to pistol
// //         //         gameObject.AddComponent<WeaponPistol>();
// //         //         break;
// //         //     }
// //         // }
    
        

// // public void SwitchWeapon(int index)
// // {
// //     if (currentWeapon != null)
// //     {
// //         Destroy(currentWeapon);
// //         currentWeapon = null;
// //     }

// //     switch (index)
// //     {
// //         case 1:
// //             currentWeapon = gameObject.AddComponent<WeaponPistol>();
// //             break;
// //         case 2:
// //             currentWeapon = gameObject.AddComponent<WeaponShotgun>();
// //             break;
// //         case 3:
// //             currentWeapon = gameObject.AddComponent<WeaponSMG>();
// //             break;
// //         case 4:
// //             currentWeapon = gameObject.AddComponent<WeaponAssaultRifle>();
// //             break;
// //         case 5:
// //             currentWeapon = gameObject.AddComponent<WeaponSniper>();
// //             break;
// //         default:
// //             currentWeapon = gameObject.AddComponent<WeaponPistol>();
// //             break;
// //     }

// //     currentWeapon.Initalize(); // Make sure this sets up camera + manager
// // }



// //         public void switchOutWeapon(int currentWeapon)  // Remove weapon component from camera/player
// //         {
            
// //             switch (currentWeapon) 
// //             {
// //             case 1:
// //                 WeaponBase pistol = GetComponent<WeaponPistol>();
// //                 pistol.unequip();
// //                 break;
// //             case 2:
// //                 // WeaponBase shotgun = GetComponent<WeaponShotgun>();
// //                 // shotgun.unequip();
// //                 break;
// //             case 3:
// //                 WeaponBase sub = GetComponent<WeaponSMG>();
// //                 sub.unequip();
// //                 break;
// //             case 4:
// //                 WeaponBase ar = GetComponent<WeaponAssaultRifle>();
// //                 ar.unequip();
// //                 break;
// //             case 5:
// //                 WeaponBase sniper = GetComponent<WeaponSniper>();
// //                 sniper.unequip();
// //                 break;
// //             default:    // Default to pistol
// //                 WeaponBase weapon = GetComponent<WeaponPistol>();
// //                 weapon.unequip();
// //                 break;
// //             }
// //         }








// //     // TODO: 1) Initalize weapon component variables     2) make variables equal to class using get commponent      3) add them to list

// //     // public class WeaponTiers : MonoBehaviour
// //     // {
// //     //     public IWeapon[] list;
// //     //     public  IWeapon pistol, shotgun, submachineGun, assaultRifle, sniper;
        
// //     //     void Start()
// //     //     {
// //     //         gameObject.AddComponent<WeaponPistol>();
// //     //         gameObject.AddComponent<WeaponShotgun>();
// //     //         gameObject.AddComponent<WeaponSubmachineGun>();
// //     //         gameObject.AddComponent<WeaponAssaultRifle>();
// //     //         gameObject.AddComponent<WeaponSniper>();
// //     //         Debug.Log("ASSHOLE");
// //     //         list[0] =  pistol = GetComponent<WeaponPistol>();
// //     //         list[1] =  shotgun = GetComponent<WeaponShotgun>();
// //     //         list[2] =  submachineGun = GetComponent<WeaponSubmachineGun>();
// //     //         list[3] =  assaultRifle = GetComponent<WeaponAssaultRifle>();
// //     //         list[4] =  sniper = GetComponent<WeaponSniper>();
        
// //     //     }
//     }

private WeaponBase currentWeapon;
private Transform weaponHolder;
[SerializeField]
private Transform propHolder;
private PlayerNetwork playerNetwork;
// private Transform ;

private void Awake()
{
    // weaponHolder = transform.Find("WeaponHolder");
    // playerNetwork = GetComponent<PlayerNetwork>();
    // if (weaponHolder == null || playerNetwork == null)
    // {
    //     Debug.Log("Child not present");
    // }

}

public override void OnNetworkSpawn()
{
     foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            NetworkObject player = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            // Transform playerHolder = player.transform.Find("propHolder");
            Debug.Log(player);
        }
    if (!IsOwner) 
    {
        // Debug.Log("Not the owner");
        return;
    }
    // weaponHolder = transform.Find("WeaponHolder");
    // Transform cam = transform.Find("MainCamera");
    // propHolder = cam.transform.Find("PropHolder");
    playerNetwork = GetComponent<PlayerNetwork>();
    if (weaponHolder == null || playerNetwork == null)
    {
        Debug.Log("Child not present");
    }
    SwitchWeapon(1);
    // currentWeapon = gameObject.AddComponent<WeaponPistol>();
    // currentWeapon.Initalize(NetworkManager.Singleton.LocalClientId,GetComponent<PlayerNetwork>());
    if (IsClient) 
    {
        // Debug.Log("I am the client");
    }
    
}

public void PlayerSpawnWithWeapon(GameObject player)
{
    // if (IsOwner) SwitchWeapon(1);
}

public void SwitchWeapon(int index)
{
if (!IsOwner) return;
    if (currentWeapon != null)
    {
        Destroy(currentWeapon);
        currentWeapon = null;
    }
    if (IsOwner)
    {
        switch (index)
        {
            case 1: propHolder.GetChild(index).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponPistol>(); break;
            case 2: propHolder.GetChild(index).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponShotgun>(); break;
            case 3: propHolder.GetChild(index).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponSMG>(); break;
            case 4: propHolder.GetChild(index).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponAssaultRifle>(); break;
            case 5: propHolder.GetChild(index).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponSniper>(); break;
            default: propHolder.GetChild(1).gameObject.SetActive(true); currentWeapon = gameObject.AddComponent<WeaponPistol>(); break;
        }

        // currentWeapon.Initalize(NetworkManager.Singleton.LocalClientId, playerNetwork);
        currentWeapon.Initalize();

    }
    // IsOwner == (NetworkObject.OwnerClientId == NetworkManager.Singleton.LocalClientId)

}

// public void Update()
// {
//     Debug.Log($"{gameObject.GetComponent<WeaponPistol>()} {gameObject.GetComponent<NetworkObject>().OwnerClientId}" );
// }
}
}

