using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class GameStart : NetworkBehaviour
{
    public static GameStart Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartPause(float duration)
    {
        if (IsServer)
        {
            PauseGameClientRpc();
            StartCoroutine(ResumeAfterDelay(duration));
        }
    }

    private IEnumerator ResumeAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResumeGameClientRpc();
    }

    [ClientRpc]
    public void PauseGameClientRpc()
    {
        Time.timeScale = 0f;
    }

    [ClientRpc]
    private void ResumeGameClientRpc()
    {
        Time.timeScale = 1f;
    }

    [ClientRpc]
    public void AppleForceToClientRpc(Vector3 direction, ClientRpcParams rprcParams = default)
    {
        Debug.Log("Client is being hit");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
                  rb.AddForce(-direction * 4f, ForceMode.Impulse);
        }
    }
}
