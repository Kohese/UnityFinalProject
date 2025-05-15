using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Netcode.Transports.UTP;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
    {
        hostBtn.onClick.AddListener(() => {
            ChangeScene();
            // NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() => {
            StartClient();
        });


    }
    void Start()
    {
        
    }

    private void ChangeScene()
    {
        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // set to 0.0.0.0 to listen to all hosts
        transport.SetConnectionData("0.0.0.0", 7777);

        NetworkManager.Singleton.StartHost();

        Debug.Log("Host started on 0.0.0.0:7777");
        // NetworkManager.Singleton.SceneManager.LoadScene("test", LoadSceneMode.Single);
        NetworkManager.Singleton.SceneManager.LoadScene("game", LoadSceneMode.Single);
    }

        private void StartClient()
    {
        // use this when creating textbox to enter ip
        // string ip = ipInput.text;

        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // set ip to string so player can input the ip
        // transport.SetConnectionData(ip, 7777);

        // need to change from 127.0.0.1 once testing is done
        transport.SetConnectionData("127.0.0.1", 7777);
        NetworkManager.Singleton.StartClient();

        hostBtn.gameObject.SetActive(false);
        clientBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
