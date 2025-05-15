using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Collections;

public class NetworkPlayerData : NetworkBehaviour
{
    public int skinIndex;
    public int idx;

    public NetworkVariable<int> syncedSkinIndex = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<FixedString64Bytes> syncedName = new NetworkVariable<FixedString64Bytes>(
        "", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public NetworkVariable<int> syncedSkin = new NetworkVariable<int>(
        0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static NetworkPlayerData Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnNetworkSpawn()
    {
        // Register for all clients to update the name
        syncedName.OnValueChanged += (oldVal, newVal) =>
        {
            gameObject.name = newVal.ToString();
            Debug.Log($"[{OwnerClientId}] Name updated to: {newVal}");
        };

        // Apply immediately if already set
        if (!string.IsNullOrEmpty(syncedName.Value.ToString()))
        {
            gameObject.name = syncedName.Value.ToString();
        }

        // Owner only runs the personal setup logic
        if (IsOwner)
        {
            StartCoroutine(WaitForLocalPlayer());
        }
    }

    private IEnumerator WaitForLocalPlayer()
    {
        GameObject playerObj = null;

        while (playerObj == null)
        {
            playerObj = NetworkManager.Singleton.LocalClient.PlayerObject?.gameObject;
            yield return null;
        }

        Debug.Log("Local player spawned, doing setup...");
        RunSetup();
    }

    private void RunSetup()
    {
        // Owner-side setup
        if (IsOwner)
        {
            DontDestroyOnLoad(this.gameObject);

            ulong ownerId = GetComponentInParent<NetworkObject>().OwnerClientId;

            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(ownerId, out var client))
            {
                var player = client.PlayerObject;
                Debug.Log(player.transform.Find("Skins"));
            }

            skinIndex++;
            idx = (int)ownerId + skinIndex + Random.Range(0, 6);
            syncedSkinIndex.Value = idx;

            // tell server to set name
            RequestNameServerRpc($"Player {ownerId} Data");
        }
    }

    [ServerRpc]
    private void RequestNameServerRpc(string name)
    {
        syncedName.Value = name;
    }

    public void SetName(string name)
    {
        if (IsServer)
            syncedName.Value = name;
    }
}

/* -------------------- OLD CODE -------------------------- */


// using UnityEngine;
// using Unity.Netcode;
// using Unity.Collections;
// using UnityEngine.SceneManagement;
// using System.Collections;
// public class NetworkPlayerData : NetworkBehaviour
// {
//     public int skinIndex;
//     public int idx;
//     public NetworkVariable<int> syncedSkinIndex = new NetworkVariable<int>(
//     0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

//     // to get the player
//         // NetworkManager.Singleton.ConnectedClients[hitClientId].PlayerObject;
//     public static NetworkPlayerData Instance;

//     private void Awake()
//     {
//         Instance = this;
//     }

//     public void OnEnable()
//     {
//             DontDestroyOnLoad(this.gameObject);
//               if (IsOwner)
//         {
//             ulong ownerId = GetComponentInParent<NetworkObject>().OwnerClientId;
//             NetworkObject player = NetworkManager.Singleton.ConnectedClients[ownerId].PlayerObject;
//             Debug.Log(player.transform.Find("Skins"));
//             skinIndex++;
//             int idx = (int)ownerId + skinIndex + Random.Range(0, 6);
//             syncedSkinIndex.Value = idx;
//         }
//     }

//     // public NetworkVariable<FixedString64Bytes> syncedName = new NetworkVariable<FixedString64Bytes>();
//     public NetworkVariable<FixedString64Bytes> syncedName = new NetworkVariable<FixedString64Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
//     public NetworkVariable<int> syncedSkin = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


//     public override void OnNetworkSpawn()
//     {
//         if (!IsOwner) return;

//         StartCoroutine(WaitForLocalPlayer());

//         // string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
//         // if (currentScene != "SampleScene") return;
//                 // This should fire on ALL clients including host
//     }

//     private IEnumerator WaitForLocalPlayer()
//     {
//         GameObject playerObj = null;

//         while (playerObj == null)
//         {
//             // Try to get the local player object
//             playerObj = NetworkManager.Singleton.LocalClient.PlayerObject?.gameObject;
//             yield return null;
//         }

//         // ✅ Player is ready — run your logic
//         Debug.Log("Local player spawned, doing setup...");

//         // Example: get a component or assign reference
//         // var playerScript = playerObj.GetComponent<PlayerNetwork>();
//         RunUpdate();
//         // DoStuffWithPlayer(playerScript);
//     }

//     private void RunUpdate()
//     {
//         {
//                    syncedName.OnValueChanged += (oldVal, newVal) =>
//         {
//             gameObject.name = newVal.ToString();
//             Debug.Log($"[{OwnerClientId}] Name updated to: {newVal}");
//         };

//         syncedSkin.OnValueChanged += (oldVal, newVal) =>
//         {
//             Debug.Log($"[{OwnerClientId}] Name updated to: {newVal}");
//         };

//         // In case the value was already set before this spawned
//         if (!string.IsNullOrEmpty(syncedName.Value.ToString()))
//         {
//             gameObject.name = syncedName.Value.ToString();
//         }
//         if (IsOwner)
//             DontDestroyOnLoad(this.gameObject);
//             syncedName.OnValueChanged += (_, newVal) => gameObject.name = newVal.ToString();
//             gameObject.name = syncedName.Value.ToString();
//             ulong ownerId = GetComponentInParent<NetworkObject>().OwnerClientId;
//             NetworkObject player = NetworkManager.Singleton.ConnectedClients[ownerId].PlayerObject;
//             Debug.Log(player.transform.Find("Skins"));
//             skinIndex++;
//             int idx = (int)ownerId + skinIndex + Random.Range(0, 6);
//             syncedSkinIndex.Value = idx;
//         }
//     }





//     public void SetName(string name)
//     {
//         if (IsServer) syncedName.Value = name;
//     }

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

