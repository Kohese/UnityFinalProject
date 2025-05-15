using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class ProjectileShoot : NetworkBehaviour
{
    public ulong shooterClientId;
    public float damage;

    // In projectile script
private void Start()
{
    StartCoroutine(EnableColliderDelayed());
}

private IEnumerator EnableColliderDelayed()
{
    Collider col = GetComponent<Collider>();
    col.enabled = false;
    yield return new WaitForSeconds(5f);
    col.enabled = true;
}
    // public ulong ownerClientId;
    // private void OnCollisionEnter(Collision collision) {
    //     //    if (!IsServer) return; // Only the server handles collision logic

    //     // Debug.Log($"Projectile hit: {collision.collider.name} | id: {collision.collider.GetComponentInParent<NetworkObject>()?.OwnerClientId}");

    //     // Example: Check if it hit a player
    //     if (IsOwner)
    //     {

    //     PlayerNetwork playerHit = collision.collider.GetComponentInParent<PlayerNetwork>();
    //     if (playerHit != null)
    //     {
    //         ulong ownerId = GetComponentInParent<NetworkObject>().OwnerClientId;
    //         Debug.Log(ownerId);
    //         if (playerHit != null && playerHit.OwnerClientId == ownerId)
    //         {
    //             Debug.Log($"Ignored self-hit | Owner ID: {ownerId} | {IsOwner}");
    //             return;
    //         }

    //         // Apply knockback via ClientRpc
    //         Vector3 knockbackDir = (playerHit.transform.position - transform.position).normalized;

    //         Debug.Log($"[Server] Hit player with ClientId {playerHit.OwnerClientId}");
    //         playerHit?.ReportHitServerRpc(NetworkManager.Singleton.LocalClientId, playerHit.OwnerClientId, knockbackDir * 10f);
    //         // GetComponent<Rigidbody>().isKinematic = true;
    //     }

    //         // ClientRpcParams rpcParams = new ClientRpcParams
    //         // {
    //         //     Send = new ClientRpcSendParams
    //         //     {
    //         //         TargetClientIds = new[] { playerHit.OwnerClientId }
    //         //     }
    //         // };

    //         // playerHit.ApplyKnockbackClientRpc(knockbackDir * 4f, rpcParams);
    //         // // playerHit.AdjustHealthServerRpc(rpcParams.Send.TargetClientIds[0]);
    //         // playerHit.AdjustHealthClientRpc(rpcParams);
    //         // if (collision.collider)
    //                 // Now destroy the projectile

    //         // if (gameObject.GetComponent<NetworkObject>().IsSpawned)
    //         // {
    //         //     gameObject.GetComponent<NetworkObject>().Despawn();
    //         // }
    //             // gameObject.GetComponent<NetworkObject>().Despawn();
                
    //     }
        
    //     // if (!collision.collider.GetComponentInParent<ProjectileShoot>())
    //     // {
    //     //  var obj = GetComponent<NetworkObject>();
    //     //     if (obj != null && obj.IsSpawned)
    //     //     {
    //     //         Debug.Log("Despawning");
    //     //         obj.Despawn();
    //     //         // Destroy(obj);
    //     //     }
    //     // }

    //         // Debug.Log(obj);
    //     // // Optionally destroy projectile
    //     // NetworkObject projectileNetObj = GetComponent<NetworkObject>();
    //     // if (projectileNetObj != null && projectileNetObj.IsSpawned)
    //     // {
    //     //     projectileNetObj.Despawn();
    //     // }
        
    // }

    private void OnCollisionEnter(Collision collision)
{
    if (!IsServer) return; // ✅ Only server handles physics collisions

     Debug.Log("Object that collided with me: " + collision.gameObject.name);

    PlayerNetwork playerHit = collision.collider.GetComponentInParent<PlayerNetwork>();
    if (playerHit != null)
    {
        ulong shooterId = GetComponent<NetworkObject>().OwnerClientId;

        if (playerHit.OwnerClientId == shooterClientId)
        {
            Debug.Log($"Ignored self-hit | Owner ID: {shooterId}");
            return;
        }

        Vector3 knockbackDir = (playerHit.transform.position - transform.position).normalized;

        Debug.Log($"[Server] Hit player with ClientId {playerHit.OwnerClientId}");
        playerHit.ReportHitServerRpc(damage, shooterClientId, playerHit.OwnerClientId, knockbackDir * 10f);
    }

    // ✅ Despawn the projectile
    NetworkObject netObj = GetComponent<NetworkObject>();
    if (netObj.IsSpawned)
    {
        netObj.Despawn();
    }
}


[ServerRpc(RequireOwnership = false)]
public void ReportHitServerRpc(ulong hitClientId, Vector3 direction)
{
    Debug.Log($"[ServerRpc] Client reported hit on {hitClientId}");
    // NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject
    PlayerNetwork playerHit = NetworkManager.Singleton.ConnectedClients[hitClientId].PlayerObject.GetComponentInParent<PlayerNetwork>();

    ClientRpcParams rpcParams = new ClientRpcParams
    {
        Send = new ClientRpcSendParams
        {
            TargetClientIds = new[] { hitClientId }
        }
    };
    // playerHit.ApplyKnockbackClientRpc(direction, rpcParams);
    // playerHit.AdjustHealthClientRpc(rpcParams);
    // ApplyKnockbackClientRpc(direction, rpcParams); 
}
}
