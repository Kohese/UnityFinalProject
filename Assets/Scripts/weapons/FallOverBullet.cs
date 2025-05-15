using UnityEngine;
using Unity.Netcode;

public class FallOverBullet : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rigidbody rb = GetComponent<Rigidbody>();
        // rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        // rb.AddTorque(Random.onUnitSphere * 2f, ForceMode.Impulse);
    }

    public void SendBullet(Vector3 direction, Rigidbody rb)
    {
        Debug.Log("SEND BULLET RUNNING");
        if (rb != null)
            {
                rb.linearVelocity = direction.normalized * 30f;
                rb.AddForce(direction.normalized * 30f, ForceMode.Impulse);
            }
    }
}
