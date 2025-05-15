using Unity.Netcode;
using UnityEngine;
using WeaponStrategyPattern;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

// how the players are spawning

public class PlayerSpawner : NetworkBehaviour
{
    private Transform spawnParent;
    public GameObject playerPrefab;
    // private NetworkPlayerData networkPlayerData;
    public WeaponTiers wp;
    [SerializeField] private string targetSceneName = "SampleScene";
    Transform randomSpawn;


    void Awake()
{
    DontDestroyOnLoad(this.gameObject);
}

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("Start");
    }
    // public override void OnNetworkSpawn()
    // {
    //         // Debug.Log("Working");

    //     if (IsHost)
    //     {
    //         NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
    //         Debug.Log("About to spawn player");
    //     }
    // }

        // When the scene is enabled - the host will add function to callback 
    public override void OnNetworkSpawn()
    {
        // skinList = playerPrefab.transform.Find("SkinList");
        // playerPrefab.transform.FindChild("skins");

        // GameObject pls = playerPrefab.FindGameObjectWithTag("SkinList");
        // Debug.Log(skinList);
            Debug.Log("Running");
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += SpawnPlayer;
        }
    }

    // when scene is disabled - host will remove function from callback
    // private void OnDisable()
    // {
    //     if (IsServer)
    //     {
    //         NetworkManager.Singleton.SceneManager.OnLoadComplete -= SpawnPlayer;
    //     }
    // }

        private void OnDestroy()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= SpawnPlayer;
    }

    private void SpawnPlayer(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log("spawning player");
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene != targetSceneName) return;


        GameObject player = Instantiate(playerPrefab,GetSpawnPosition(), Quaternion.identity);
        player.name = $"Player {clientId}";
        // player.transform.position = GetSpawnPosition();
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);

            // Set initial weapon index after spawn
        // var weaponHandler = player.GetComponent<PlayerWeaponTierTracker>();
        // weaponHandler.currentWeaponIndex.Value++;
        

        // if (IsServer)
        // {
            // NetworkPlayerData data = GetData(clientId);
            ClientRpcParams rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { clientId }
                }
            };
            ApplySkinClientRpc(clientId, rpcParams);
        // }

        // player.GetComponent<PlayerNetwork>().ApplySkin()
        // wp = player.GetComponent<WeaponTiers>();
        // wp.PlayerSpawnWithWeapon(player);

        // Freeze game after spawn for 3 seconds
        GameStart.Instance.StartPause(3f);
    }


    [ClientRpc]
    private void ApplySkinClientRpc(ulong clientRpcId, ClientRpcParams rpcParams)
    {
        if (NetworkManager.Singleton.LocalClientId != clientRpcId) return;

        StartCoroutine(WaitForPlayerToSpawn(clientRpcId));

        NetworkPlayerData data = GetData(clientRpcId);
        if (data != null) Debug.Log(data);
    }

    private IEnumerator WaitForPlayerToSpawn(ulong clientId)
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null);

        NetworkPlayerData data = GetData(clientId);
        if (data != null) Debug.Log(data.skinIndex);
        Transform skins = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.transform.Find("Skins");
        Debug.Log(skins);
        // Debug.Log("running the client rpc");
    }

    private NetworkPlayerData GetData(ulong clientId)
    {
        // ulong clientId = NetworkManager.Singleton.LocalClientId;
        string expectedName = $"Player {clientId} Data";
        ulong targetClientId = clientId;
        var playerDataObject = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList
        .FirstOrDefault(obj =>
            obj.OwnerClientId == targetClientId &&
            obj.name == expectedName);

    if (playerDataObject != null)
    {
        GameObject playerData = playerDataObject.gameObject;
        Debug.Log($"✅ Found PlayerData for Client {targetClientId}: {playerData.name}");
        var playerVars = playerData.GetComponent<NetworkPlayerData>();
        return playerVars;
    }
    return null;
    }

    //
// private Vector3 GetSpawnPosition()
// {
//     spawnParent = GameObject.FindGameObjectWithTag("Spawns").transform;
//     if (spawnParent == null || spawnParent.childCount == 0)
//     {
//         Debug.LogWarning("No spawn points found.");
//         return Vector3.zero;
//     }

//     int randomIndex = Random.Range(0, spawnParent.childCount);
//     Transform randomSpawn = spawnParent.GetChild(randomIndex);

//     Debug.Log($"Spawning at: {randomSpawn.name} {randomSpawn.position}");
//     return randomSpawn.position;
// }
     // }

         private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-10f, 10f), 3f, Random.Range(-10f, 10f));
    }

}
