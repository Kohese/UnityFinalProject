using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Linq;
using Microlight.MicroBar;



public class PlayerNetwork : NetworkBehaviour {

    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform muzzleShotgun;
    [SerializeField] private Transform muzzleSMG;
    [SerializeField] private Transform muzzleAR;
    [SerializeField] private Transform muzzleSniper;
    [SerializeField] private Transform propHolder;
    public Transform MuzzlePoint => muzzlePoint;
    public Transform MuzzleShotgun => muzzleSniper;
    public Transform MuzzleSMG => muzzleSMG;
    public Transform MuzzleAR => muzzleAR;
    public Transform MuzzleSniper => muzzleSniper;
    [SerializeField] MicroBar bar;

    public NetworkVariable<float> health = new NetworkVariable<float>(50, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> healths = new NetworkVariable<float>(50, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, default, NetworkVariableWritePermission.Server);
    private GameObject DataSource;


    public override void OnNetworkSpawn()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        // if (!IsOwner) return;
        Transform skins = transform.Find("Skins");

        if (IsClient)
        {
            Debug.Log("I AM SPAWNING");
            StartCoroutine(WaitForPlayerToSpawn());
            bar.Initialize(healths.Value);
        } 
        healths.OnValueChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float previous, float current)
    {
        Debug.Log($"[Client] Health changed: {previous} → {current}");
        bar.UpdateBar(current); // Update the local UI bar on all clients
    }


        private IEnumerator WaitForPlayerToSpawn()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
                // ulong targetClientId = clientId; // The client you're looking for
        string expectedName = $"Player {clientId} Data"; // Match this to what you named it

        foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
        {

        Debug.Log($"I FAIL AT 0: {id}");
            yield return new WaitUntil(() => NetworkManager.Singleton.SpawnManager.SpawnedObjectsList
            .Any(obj =>
                obj.OwnerClientId == id &&
                obj.name == $"Player {id} Data"));

            var playerDataObject = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList
            .FirstOrDefault(obj =>
                obj.OwnerClientId == id &&
                obj.name == $"Player {id} Data");

        yield return new WaitUntil(() => playerDataObject != null);
        var playerVars = playerDataObject.gameObject.GetComponent<NetworkPlayerData>();

        yield return new WaitUntil(() => playerVars != null);
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
        Transform skins = player.transform.Find("Skins");
        Transform holder = player.transform.Find("propHolder");
        Debug.Log($"Holder: {holder}");

         int index = playerVars.syncedSkin.Value;
         Debug.Log($"Here is the index: {index}");

    // Deactivate all first to be safe
        for (int i = 0; i < skins.childCount; i++)
            skins.GetChild(i).gameObject.SetActive(i == index);
        }
//         }
    // Transform skins = transform.Find("Skins");
    //     Debug.Log("FOUND");
    //     Debug.Log("I FAIL AT 1");
    //     Debug.Log("I FAIL AT 2");
        
    //     Debug.Log($"STILL RUNNING: {playerVars.syncedSkinIndex.Value}");

    //     // NetworkPlayerData data = GetData(clientId);
    //     // if (data != null) Debug.Log(data.skinIndex);
    //      if (skins == null)
    // {
    //     Debug.LogWarning("Skins transform not found!");
    //     yield break;
    // }

    // int index = playerVars.syncedSkinIndex.Value;

    // // Deactivate all first to be safe
    // for (int i = 0; i < skins.childCount; i++)
    //     skins.GetChild(i).gameObject.SetActive(i == index);

        // if (IsOwner)
        // {
        //     for (int i = 0; i < skins.childCount; i++)
        //     {
        //     skins.GetChild(playerVars.syncedSkinIndex.Value).gameObject.SetActive(true);
        //     Debug.Log($"THIS IS THE SKIN TO APPLy: {playerVars.idx} | {playerVars.syncedSkinIndex.Value}");
        //     skins.GetChild(0).gameObject.SetActive(false);
        //     }
        // }
        // else
        // {
        //     Debug.Log("Not the owner");
        //     for (int i = 0; i < skins.childCount; i++)
        //     {
        //         skins.GetChild(playerVars.syncedSkinIndex.Value).gameObject.SetActive(true);
        //         Debug.Log($"THIS IS THE SKIN TO APPLy: {playerVars.idx} | {playerVars.syncedSkinIndex.Value}");
        //         skins.GetChild(0).gameObject.SetActive(false);
        //     }
        // }
            // for (int i = 0; i < skins.childCount; i++)
            // {
            // skins.GetChild(playerVars.syncedSkinIndex.Value).gameObject.SetActive(true);
            // skins.GetChild(0).gameObject.SetActive(false);
            // }
        // Debug.Log(skins);/
        // Debug.Log("running the client rpc");
    }

    private void GetPlayerData()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
            ulong targetClientId = clientId; // The client you're looking for
    string expectedName = $"Player {clientId} Data"; 

    var playerDataObject = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList
        .FirstOrDefault(obj =>
            obj.OwnerClientId == targetClientId &&
            obj.name == expectedName);

    if (playerDataObject != null)
    {
        GameObject playerData = playerDataObject.gameObject;
        Debug.Log($"✅ Found PlayerData for Client {targetClientId}: {playerData.name}");
        var playerVars = playerData.GetComponent<NetworkPlayerData>();
        Transform skins = gameObject.transform.Find("Skins");
        if (IsOwner && playerVars != null)
        {
         if (skins != null)
            {
                for (int i = 0; i < skins.childCount; i++)
                {
                    skins.GetChild(playerVars.idx).gameObject.SetActive(true);
                    // Debug.Log($"Child {i}: {child.name}");
                }
            }
        }
        else
        {
            Debug.Log("Not owner");
        }

    }
    else
    {
        Debug.LogWarning($"PlayerData not found for Client {targetClientId}");
    }
    }

    public void Update()
    {


    }

    public void ApplyForce(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float multiplier = IsHost ? 10f : 10f; // compensate for client physics being weaker

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;

            // Flatten the Y axis to prevent vertical launching in the air
            direction.y = 0f;
            direction.Normalize();

            Debug.Log($"IsOwner: {IsOwner}, LocalClientId: {NetworkManager.Singleton.LocalClientId} | direction: {direction}");

            rb.AddForce(direction * 3f * multiplier, ForceMode.Impulse);

            Debug.Log($"IsOwner: {IsOwner}, LocalClientId: {NetworkManager.Singleton.LocalClientId} |  direction: {direction}");
        }
    }

    public void ShootGun(Vector3 origin, Vector3 direction)
    {
        // Debug.Log(OwnerClientId);
        if (!IsOwner)
        {
            // Debug.Log("You are not owner");
            return; 
        } 

        // if (IsOwner)
        // {
        //     ShootRayServerRpc(origin, direction);
        // }
    }

public void ApplyKnockback(Vector3 direction)
{
    if (!IsOwner) return; // Only owner applies force

    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(direction.normalized * 40f, ForceMode.Impulse);
    }
}
    [SerializeField] private GameObject hitSpherePrefab;

    // to modify the player shooting raycast / prefab stuff
    // does not destroy the spheres - need to do next
    [ServerRpc]
    public void ShootRayServerRpc(float damage, Vector3 origin, Vector3 direction, ServerRpcParams serverRpcParams = default)
    {
        if (OwnerClientId != serverRpcParams.Receive.SenderClientId) return;
        Debug.Log("I am shooting");
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (hitSpherePrefab != null)
        {
            Quaternion bulletRotation = Quaternion.LookRotation(direction.normalized);
            bulletRotation *= Quaternion.Euler(90f, 0f, 0f); // Adjust this to match your model orientation
            GameObject sphere = Instantiate(hitSpherePrefab, origin, bulletRotation);
            sphere.GetComponent<NetworkObject>().Spawn();
            sphere.layer = LayerMask.NameToLayer("Bullet");
            var proj = sphere.GetComponent<ProjectileShoot>();
            proj.shooterClientId = clientId;
            proj.damage = damage;

            // Ignore collision with shooter's collider
            var shooterObj = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var shooterColliders = shooterObj.GetComponentsInChildren<Collider>();
            var projectileColliders = sphere.GetComponentsInChildren<Collider>();

            foreach (var sc in shooterColliders)
            {
                foreach (var pc in projectileColliders)
                {
                    Physics.IgnoreCollision(sc, pc, true);
                }
            }

            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            sphere.AddComponent<FallOverBullet>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * 30f;
            }
        
        }
    }

        private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
    }


    [ClientRpc]
    public void ApplyKnockbackClientRpc(Vector3 direction, ClientRpcParams rpcParams)
    {
        if (IsOwner)
        {
            Debug.Log($"Client is being hit: {NetworkManager.Singleton.LocalClientId}");
            ApplyForce(direction);
        }
    }

    [ServerRpc]
    public void AdjustHealthServerRpc(ulong attacker, ulong id, ServerRpcParams serverRpcParams = default)
    {
            // healths.Value--;
            Debug.Log($"This is your health client: {id}: {healths.Value}");

            Vector3 hitDirection = (transform.position - NetworkManager.Singleton.ConnectedClients[attacker].PlayerObject.transform.position).normalized;
            
            Vector3 knockbackForce = (hitDirection + Vector3.up * 0.5f).normalized * 7f;

            // if (healths.Value <= 0 && !GetComponent<PlayerWeaponTierTracker>().isDead.Value)
            if (healths.Value <= 0 && (GetComponent<PlayerMovement>().isAlive.Value == true))
            {
                AwardKill(attacker);
                AddDeath(hitDirection);
            }
            if(healths.Value <= 0 && (GetComponent<PlayerMovement>().isAlive.Value == false))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.freezeRotation = false;
            }
    }
    [ClientRpc]
    public void PlayDeathKnockbackClientRpc(Vector3 force)
    {
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }


    [ClientRpc]
    public void AdjustHealthClientRpc(float damage, ulong attacker, ClientRpcParams ClientRpcParams)
    {
        // if (id != NetworkManager.Singleton.LocalClientId) return;
        // healths.Value -= damage;
        bar.UpdateBar(bar.CurrentValue - damage);
        if (IsOwner)
        {
            Debug.Log("I am applying this script");
        // if (id != NetworkManager.Singleton.LocalClientId) return;
            AdjustHealthServerRpc(attacker, NetworkManager.Singleton.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReportHitServerRpc(float damage, ulong attacker, ulong hitClientId, Vector3 direction)
    {
        Debug.Log($"[ServerRpc] Client reported hit on {hitClientId} shot from {attacker}");
        // NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject
        PlayerNetwork playerHit = NetworkManager.Singleton.ConnectedClients[hitClientId].PlayerObject.GetComponentInParent<PlayerNetwork>();
    if (NetworkManager.Singleton.ConnectedClients.TryGetValue(hitClientId, out var client))
    {
        var stats = client.PlayerObject.GetComponent<PlayerWeaponTierTracker>();
        stats?.ApplyDamage(damage); // Implement AddKill() in PlayerStats
    }


        ClientRpcParams rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { hitClientId }
            }
        };
        playerHit.ApplyKnockbackClientRpc(direction, rpcParams);
        playerHit.AdjustHealthClientRpc(damage, attacker, rpcParams);
        // ApplyKnockbackClientRpc(direction, rpcParams); 
    }

    private void AwardKill(ulong attackerClientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(attackerClientId, out var client))
        {
            var stats = client.PlayerObject.GetComponent<PlayerWeaponTierTracker>();
            stats?.AddKill(attackerClientId); // Implement AddKill() in PlayerStats
            
        }
    }
    private void AddDeath(Vector3 hitDirection)
    {
        var stats = GetComponent<PlayerWeaponTierTracker>();
        stats?.AddDeath(hitDirection); // Implement AddKill() in PlayerStats
        // hitDirection
    }

    [ClientRpc]
public void TeleportClientRpc(Vector3 newPosition)
{
    transform.position = newPosition;
}

[ClientRpc]
public void SetDeadStateClientRpc(bool isDead)
{
    // disable movement
    GetComponent<PlayerMovement>().enabled = !isDead;
}
}