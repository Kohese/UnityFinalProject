using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using UnityEngine;

public class IPHelper : MonoBehaviour
{
    void Start()
    {
        string ip = GetLocalIPv4();
        Debug.Log("Local IP: " + ip);
    }

    public static string GetLocalIPv4()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            var ipProps = ni.GetIPProperties();
            foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
            {
                if (addr.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(addr.Address))
                {
                    return addr.Address.ToString(); // ✅ Real IPv4 LAN address
                }
            }
        }

        return "No network adapter with IPv4 found";
    }
}
