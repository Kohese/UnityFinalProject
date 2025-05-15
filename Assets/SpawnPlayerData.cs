using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class SpawnPlayerData : NetworkBehaviour
{
    [SerializeField] private GameObject PlayerDataPrefab;
    // public NetworkVariable<string> PlayerName = new NetworkVariable<string>();
    public override void OnNetworkSpawn()
    {
        // PlayerName.OnValueChanged += (oldValue, newValue) => {
        //     gameObject.name = newValue;
        // }
        // gameObject.name = PlayerName.Value;
        if (IsServer) NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    // private void SetName(GameObject data, string name)
    // {
    //     if (IsServer) data.name = name;
    // }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer) return;
        if (IsServer)
        {
        GameObject PlayerData = Instantiate(PlayerDataPrefab);
        // PlayerData.name = PlayerName.Value;
        // var nameSetter = PlayerData.GetComponent<NetworkPlayerData>();
        // nameSetter.SetName($"Player {clientId} Data");
        PlayerData.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        var nameSync = PlayerData.GetComponent<NetworkPlayerData>();
        nameSync.syncedName.Value = $"Player {clientId} Data";
        nameSync.syncedSkin.Value = Random.Range(1, 7);
        // SetNameClientRpc(PlayerData, $"Player {clientId} Data");
        // PlayerData.name = $"Player {clientId} Data";
        // StartCoroutine(ShowDataToAll(PlayerData.GetComponent<NetworkObject>()));


//         foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
// {
//     // networkObject.NetworkShow(clientId); // ✅ one at a time
//         PlayerData.GetComponent<NetworkObject>().NetworkShow(id); // show to everyone
// }
//         }


    }
    }
    [ClientRpc]
    private void SetNameClientRpc(NetworkObjectReference player, string newName)
    {
        // if (IsServer)
        Debug.Log("Spawner SetNameClientRpc: " + newName);

        if (IsServer)
        {
        ((GameObject)player).name = newName;
        ((GameObject)player).GetComponent<NetworkPlayerData>().syncedName.Value = newName;

        }
    }
    private IEnumerator ShowDataToAll(NetworkObject netObj)
{
    yield return null; // wait one frame to ensure spawn completes

    foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
    {
        if (!netObj.IsNetworkVisibleTo(id))
        {
            netObj.NetworkShow(id);
        }
    }
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}