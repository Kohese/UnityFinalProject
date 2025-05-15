// using UnityEngine;
// using Unity.Netcode;
// using WeaponStrategyPattern;
// using Unity.Collections;
// using System.Collections;
// using Unity.Netcode.Components;
// using JetBrains.Annotations;
// using Microlight.MicroBar;

// // going to be used to track the client gun tier

// // using network variable to store the index of the current weapon
// // weapon index will increase on every kill + use an onchange listener to update gun prefab

// public class PlayerWeaponTierTracker : NetworkBehaviour {
//     public NetworkVariable<int> currentWeaponIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
//     public NetworkVariable<int> kills = new NetworkVariable<int>(0, default, NetworkVariableWritePermission.Server);
//     public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, default, NetworkVariableWritePermission.Server);
//     public NetworkVariable<float> damage = new NetworkVariable<float>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
//     [SerializeField] private Transform propHolder;

//         public NetworkVariable<int> killCount = new NetworkVariable<int>(
//         0,
//         NetworkVariableReadPermission.Everyone,
//         NetworkVariableWritePermission.Server
//     );

//     public NetworkVariable<int> deathCount = new NetworkVariable<int>(
//         0,
//         NetworkVariableReadPermission.Everyone,
//         NetworkVariableWritePermission.Server
//     );
    
//     [SerializeField] private Camera playerCam;
//     WeaponSwitchingLogic switcher;
//     WeaponTiers w;
//      private Rigidbody rb;
//      private PlayerMovement movement;


//     private GameObject scoreTrackerObj;
//     private ScoreTracker scoreTrackerScript;
//     [SerializeField] MicroBar bar;
    
    
//     public void SetWeaponIndex(int index)
//     {
//         if (IsOwner)
//         {
//             SubmitWeaponIndexServerRpc(index);
//         }
//     }

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody>();
//         movement = GetComponent<PlayerMovement>();
//     }

//     public override void OnNetworkSpawn()
//     {

//         scoreTrackerObj = GameObject.Find("ScoreTracker");
//         scoreTrackerScript = scoreTrackerObj.GetComponent<ScoreTracker>();
//         scoreTrackerScript.AddPlayerRpc(OwnerClientId);
//                 // Register listener for weapon index sync
//         currentWeaponIndex.OnValueChanged += OnWeaponChanged;

//         // weapon appears on spawn
//         OnWeaponChanged(0, currentWeaponIndex.Value);


//         if (IsServer) currentWeaponIndex.Value = 1;
//         if (!IsOwner) return;
//         w = GetComponent<WeaponTiers>();
//         // switcher = playerCam.GetComponent<WeaponSwitchingLogic>();
//         // Debug.Log($"Weapon Tiers: {w} | switcher: {switcher}");
//         Debug.Log($"IsOwner: {IsOwner}, OwnerClientId: {NetworkObject.OwnerClientId}, LocalClientId: {NetworkManager.Singleton.LocalClientId}");
//         killCount.OnValueChanged += (_, newVal) => Debug.Log($"client: {NetworkManager.Singleton.LocalClientId} now has {newVal} kills");
//         deathCount.OnValueChanged += (_, newVal) => Debug.Log($"client: {NetworkManager.Singleton.LocalClientId} now has {newVal} deaths");
//         isDead.OnValueChanged += DeadRespawn;
//         // damage.OnValueChanged += Apply;
//         // deathCount.OnValueChanged += (_, newVal) => AddDeath;
//         // currentWeaponIndex.OnValueChanged += OnWeaponChanged;
//         // OnWeaponChanged(0, currentWeaponIndex.Value);
//         // if (IsOwner && IsClient) currentWeaponIndex.OnValueChanged += HandleWeaponChange;

//     }

// //     public void Apply(float oldVal, float newVal)
// // {
// //         // GetComponent<PlayerNetwork>()
// //         GetComponent<PlayerNetwork>().healths.Value -= newVal;
// //         bar.UpdateBar(bar.CurrentValue - newVal);

// //         Debug.Log($"Player getting hit: {NetworkManager.Singleton.LocalClientId}");
// // }

    

// public void ApplyDamage(float dam)
// {
//         // GetComponent<PlayerNetwork>()
//         damage.Value = dam;
//         GetComponent<PlayerNetwork>().healths.Value -= dam;
//         bar.UpdateBar(bar.CurrentValue - dam);

//         Debug.Log($"Player getting hit: {NetworkManager.Singleton.LocalClientId}");

//     // if (healths.Value <= 0 && !GetComponent<PlayerWeaponTierTracker>().isDead.Value)
//     // {
//     //     AwardKill(attackerClientId);
//     //     AddDeath(direction);
//     // }
// }

//      private void OnWeaponChanged(int oldIndex, int newIndex)
//     {
//         if (newIndex <= 5)
//         {
//         Debug.Log("I am running");
//         propHolder.GetChild(oldIndex).gameObject.SetActive(false);
//         // Enable the selected weapon model, disable others
//         for (int i = 0; i < propHolder.childCount; i++)
//         {
//             propHolder.GetChild(i).gameObject.SetActive(i == newIndex);
//         }
//         }
//     }

//     public void AddKill()
//     {
//         if (movement.isAlive.Value == true)
//         {
//             killCount.Value++;
//             currentWeaponIndex.Value++;
//             scoreTrackerScript.UpdatePlayerScoreRpc(OwnerClientId, killCount.Value);
//             Debug.Log($"[Server] Client {OwnerClientId} now has {killCount.Value} kills.");
//         }
        

//         // if (killCount.Value <= propHolder.childCount)
//         // {
//         //     propHolder.GetChild(currentWeaponIndex.Value).gameObject.SetActive(true);
//         // }
//     }

//     // public void DeadRespawn(bool oldVal, bool newVal)
//     // {
//     //     StartCoroutine(RespawnAfterDelay(NetworkManager.Singleton.LocalClientId, 3f));
//     // }

//     // [Rpc(SendTo.Everyone, RequireOwnership = false)]
//     public void AddDeath(Vector3 direction)
//     {
//         isDead.Value = true;

//         if (movement.isAlive.Value == true)
//         {
//             Debug.Log("HEY ARE DEAD");
//             deathCount.Value++;
//             float knockForce = 10f;
//             movement.isAlive.Value = false; // disable movement

//             Rigidbody rb = GetComponent<Rigidbody>();
//             rb.freezeRotation = false;
//             // rb.AddForce(direction.normalized * knockForce, ForceMode.Impulse);

//             // StartCoroutine(RespawnAfterDelay(NetworkManager.Singleton.LocalClientId, 3f));

//             // StartCoroutine(FreezeRigidbodyAfterDelay(rb, 0.5f));
//         }
        
        
//         // Disable movement
//         // if (movement != null)
//         //     movement.enabled = false;

//         // rb.constraints = RigidbodyConstraints.None; // or FreezeRotation if needed
// // 

//         // // Remove constraints to allow ragdoll-like fall
//         // rb.constraints = RigidbodyConstraints.None;

//         // // Clear velocity before applying new force
//         // rb.linearVelocity = Vector3.zero;

//         // // Apply knockback force from last hit
//         // Vector3 force = (Vector3.up + lastHitDirection.normalized) * 7f;
//         // rb.AddForce(force, ForceMode.Impulse);

//         // // Log kill (you can still call AwardKill here)
//         // Debug.Log($"Player {OwnerClientId} died due to client {lastAttackerId}");

//         Debug.Log($"[Server] Client {OwnerClientId} now has {deathCount.Value} deaths.");
//     }

//      private void DeadRespawn(bool oldVal, bool newVal)
//     {
//         if (newVal == true)
//             StartCoroutine(RespawnAfterDelay(NetworkManager.Singleton.LocalClientId, 3f));
//     }

//     private IEnumerator RespawnAfterDelay(ulong clientId, float delay)
//     {
//         Debug.Log("⏳ Respawn countdown started");

//         var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
//         var player = playerObject.GetComponent<PlayerNetwork>();
//         var tracker = playerObject.GetComponent<PlayerWeaponTierTracker>();

//         player.SetDeadStateClientRpc(true); // Hide or freeze

//         yield return new WaitForSeconds(delay);

//         Vector3 spawnPos = GetSpawnPosition();

//         // Call ServerRpc to actually change NetworkVariables
//         ApplyRespawnStateServerRpc(clientId, spawnPos);
//     }

//     [ServerRpc(RequireOwnership = false)]
//     private void ApplyRespawnStateServerRpc(ulong clientId, Vector3 spawnPos)
//     {
//         var player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerNetwork>();
//         var tracker = player.GetComponent<PlayerWeaponTierTracker>();
//         var movement = player.GetComponent<PlayerMovement>();

//         player.healths.Value = 100f;
//         tracker.isDead.Value = false;
//         movement.isAlive.Value = true;

//         player.TeleportClientRpc(spawnPos);
//         player.SetDeadStateClientRpc(false);

//         Debug.Log($"✅ [Server] Player {clientId} respawned at {spawnPos}");
//     }


//     // private Vector3 GetSpawnPosition()
//     // {
//     //     return new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
//     // }

//     [ClientRpc]
//     public void TeleportClientRpc(Vector3 newPosition)
//     {
//         transform.position = newPosition;
//     }

//     [ClientRpc]
//     public void SetDeadStateClientRpc(bool isDead)
//     {
//         if (TryGetComponent(out PlayerMovement move)) move.enabled = !isDead;

//         foreach (var r in GetComponentsInChildren<Renderer>())
//             r.enabled = !isDead;
//     }

//             private Vector3 GetSpawnPosition()
//     {
//         return new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
//     }

//     private IEnumerator FreezeRigidbodyAfterDelay(Rigidbody rb, float delay)
//     {
//         yield return new WaitForSeconds(delay);

        
//         // rb.freezeRotation = false;
//         // Freeze all movement and rotation
//         // rb.linearVelocity = Vector3.zero;
//         // rb.angularVelocity = Vector3.zero;
//         // rb.constraints = RigidbodyConstraints.FreezeAll;
//     }

//     // public void OnEnable()
//     // {
//     //     Debug.Log("Enable running");
//     // }
//     // public void OnDisable()
//     // {
//     //     currentWeaponIndex.OnValueChanged -= HandleWeaponChange;
//     // }

//     private void HandleWeaponChange(int oldIdx, int newIdx)
//     {
//         // w = GetComponent<WeaponTiers>();
//         Debug.Log("I think its working");
//         // weaponTiers.switchOutWeapon();
//         w.SwitchWeapon(newIdx);
//         // Debug.Log("this is the issue");
//         SubmitWeaponIndexServerRpc(newIdx);
//     }

//     [ServerRpc]
//     private void SubmitWeaponIndexServerRpc(int index, ServerRpcParams serverRpcParams = default)
//     {
//             // currentWeaponIndex.Value = index;
//             Debug.Log(index); 
//         // if (clientId != OwnerClientId) return;
//         // if (index >= 5) 
//         // {
//         //     Debug.Log("Player wins");
//         //     GameStart.Instance.StartPause(3f);
//         // }
//         Debug.Log($"[Server] Player {OwnerClientId} switched to weapon {index}");
//         if (w != null) w.SwitchWeapon(index);
//         // switcher.SwitchWeapon(index);
//     }
// }


using UnityEngine;
using Unity.Netcode;
using WeaponStrategyPattern;
using Microlight.MicroBar;
using System.Collections;

public class PlayerWeaponTierTracker : NetworkBehaviour
{
    public NetworkVariable<int> currentWeaponIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> killCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> deathCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> damage = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private MicroBar bar;
    [SerializeField] private Transform propHolder;
    [SerializeField] private Camera playerCam;
    private GameObject scoreTrackerObj;
    private ScoreTracker scoreTrackerScript;

    private WeaponTiers w;
    private PlayerMovement movement;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    public override void OnNetworkSpawn()
    {

        scoreTrackerObj = GameObject.Find("ScoreTracker");
        scoreTrackerScript = scoreTrackerObj.GetComponent<ScoreTracker>();
        scoreTrackerScript.AddPlayerRpc(OwnerClientId);

        if (IsServer) currentWeaponIndex.Value = 1;

        currentWeaponIndex.OnValueChanged += OnWeaponChanged;
        OnWeaponChanged(0, currentWeaponIndex.Value);

        killCount.OnValueChanged += (_, newVal) => Debug.Log($"[Client {NetworkManager.Singleton.LocalClientId}] Kills: {newVal}");
        deathCount.OnValueChanged += (_, newVal) => Debug.Log($"[Client {NetworkManager.Singleton.LocalClientId}] Deaths: {newVal}");
        isDead.OnValueChanged += DeadRespawn;

        if (IsOwner)
        {
            w = GetComponent<WeaponTiers>();
            bar.Initialize(GetComponent<PlayerNetwork>().healths.Value);
        }
    }

    private void OnWeaponChanged(int oldIndex, int newIndex)
    {
        for (int i = 0; i < propHolder.childCount; i++)
            propHolder.GetChild(i).gameObject.SetActive(i == newIndex);
    }

    public void ApplyDamage(float dam)
    {
        var net = GetComponent<PlayerNetwork>();
        net.healths.Value -= dam;
        bar.UpdateBar(bar.CurrentValue - dam);
        Debug.Log($"Player {OwnerClientId} took {dam} damage");

        if (net.healths.Value <= 0 && !isDead.Value)
        {
            isDead.Value = true;
        }
    }

    public void AddKill(ulong client)
    {
        if (!movement.isAlive.Value) return;

        Debug.Log($"THis is the client: {client}");
        killCount.Value++;
        currentWeaponIndex.Value++;
        scoreTrackerScript?.UpdatePlayerScoreRpc(OwnerClientId, killCount.Value);
    }

    public void AddDeath(Vector3 direction)
    {
        if (!IsServer || isDead.Value) return;

        isDead.Value = true;
        deathCount.Value++;
        movement.isAlive.Value = false;

        Vector3 knockbackForce = direction.normalized * 10f;
        ApplyKnockbackClientRpc(knockbackForce);

        Debug.Log($"[Server] Player {OwnerClientId} died");
        Debug.Log($"This SHIT IS STUPID {movement.isAlive.Value}");
    }

    private void DeadRespawn(bool oldVal, bool newVal)
    {
        if (IsServer && newVal)
        {
            StartCoroutine(RespawnAfterDelay(OwnerClientId, 3f));
        }
    }

    private IEnumerator RespawnAfterDelay(ulong clientId, float delay)
    {
        Debug.Log($"Respawning player {clientId} in {delay} seconds");

        PlayerNetwork player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerNetwork>();
        player.SetDeadStateClientRpc(true); // Hide & disable movement

        yield return new WaitForSeconds(delay);

        Vector3 spawnPos = GetSpawnPosition();

        ApplyRespawnStateServerRpc(clientId, spawnPos);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyRespawnStateServerRpc(ulong clientId, Vector3 spawnPos)
    {
        var playerObj = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var player = playerObj.GetComponent<PlayerNetwork>();
        var tracker = playerObj.GetComponent<PlayerWeaponTierTracker>();
        var movement = playerObj.GetComponent<PlayerMovement>();

        player.healths.Value = 100f;
        tracker.isDead.Value = false;
        movement.isAlive.Value = true;

        player.TeleportClientRpc(spawnPos);
        player.SetDeadStateClientRpc(false);

        Debug.Log($"✅ [Server] Respawned player {clientId} at {spawnPos}");
    }

    [ClientRpc]
    public void ApplyKnockbackClientRpc(Vector3 force)
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.freezeRotation = false;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    [ClientRpc]
    public void TeleportClientRpc(Vector3 newPosition)
    {
        Debug.Log($"[Client {NetworkManager.Singleton.LocalClientId}] Teleporting to {newPosition}");
        transform.position = newPosition;
    }

    [ClientRpc]
    public void SetDeadStateClientRpc(bool isDead)
    {
        if (TryGetComponent(out PlayerMovement move)) move.enabled = !isDead;

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = !isDead;
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
    }

    [ServerRpc]
    private void SubmitWeaponIndexServerRpc(int index, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"[Server] Player {OwnerClientId} switched to weapon {index}");
        w?.SwitchWeapon(index);
    }
}
