using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerRespawnManager : NetworkBehaviour
{
    public void StartRespawn(ulong clientId)
    {
        StartCoroutine(RespawnAfterDelay(clientId, 3f));
    }

    private IEnumerator RespawnAfterDelay(ulong clientId, float delay)
    {
        var player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerNetwork>();

        // Disable player visuals and input
        player.SetDeadStateClientRpc(true);

        yield return new WaitForSeconds(delay);

        // Move player to spawn point
        Vector3 spawnPoint = GetRandomSpawnPosition();
        player.TeleportClientRpc(spawnPoint);

        // Reset health and state
        player.healths.Value = 100f;
        player.GetComponent<PlayerWeaponTierTracker>().isDead.Value = false;

        // Re-enable visuals and input
        player.SetDeadStateClientRpc(false);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-5f, 5f), 2f, Random.Range(-5f, 5f));
    }
}
