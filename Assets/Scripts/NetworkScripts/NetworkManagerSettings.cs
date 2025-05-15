using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class NetworkManagerSettings : MonoBehaviour
{
    public TMP_InputField ipInput;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    private void Awake()
    {
        // use these buttons to create host / join client
        hostBtn.onClick.AddListener(() => {
            StartHost();
        });
        clientBtn.onClick.AddListener(() => {
            StartClient();
        });

    }

    private void StartHost()
    {
        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // set to 0.0.0.0 to listen to all hosts
        transport.SetConnectionData("0.0.0.0", 7777);

        NetworkManager.Singleton.StartHost();

        Debug.Log("Host started on 0.0.0.0:7777");

        hostBtn.gameObject.SetActive(false);
        clientBtn.gameObject.SetActive(false);
    }

    private void StartClient()
    {
        // use this when creating textbox to enter ip
        string ip = ipInput.text;

        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // set ip to string so player can input the ip
        // transport.SetConnectionData(ip, 7777);

        // need to change from 127.0.0.1 once testing is done
        transport.SetConnectionData(ip, 7777);
        NetworkManager.Singleton.StartClient();

        hostBtn.gameObject.SetActive(false);
        clientBtn.gameObject.SetActive(false);
    }
}
