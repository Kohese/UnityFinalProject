// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using Unity.Netcode;
// using Unity.Netcode.Transports.UTP;
// using UnityEngine.SceneManagement;
// using Unity.Netcode;


// public class MainMenuUIManager : NetworkBehaviour
// {
//     [Header("Panels")]
//     public GameObject mainMenuPanel;
//     public GameObject clientPanel;
//     public GameObject lobbyPanel;

//     [Header("Input")]
//     public TMP_InputField ipInputField;

//     [Header("LobbyUI")]
//     public TextMeshProUGUI ipText;
//     public GameObject playerListContainer;
//     public GameObject playerListEntryPrefab;

    

//     private void Start()
//     {
//         ShowMainMenu();
//     }

//     public void OnHostClicked()
//     {
//         var manager = NetworkManager.Singleton;

//         if (manager == null) return;

//         if (NetworkManager.Singleton == null)
//         {
//             Debug.LogError("NetworkManager.Singleton is null!");
//             return;
//         }

//         var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
//         transport.SetConnectionData("0.0.0.0", 7777);

//         //Test lines to see about approving spawn
//         // manager.NetworkConfig.ConnectionApproval = true;
//         // manager.ConnectionApprovalCallback = ApproveConnection;

//         manager.StartHost();
//         mainMenuPanel.SetActive(false);
//         lobbyPanel.SetActive(true);
//         UpdateIPDisplay();
//     }


//     // public void OnStartGameClicked()
//     // {
//     //     if (!NetworkManager.Singleton.IsHost) return;

//     //     NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
//     // }

//     public void OnClientClicked()
//     {
//         mainMenuPanel.SetActive(false);
//         clientPanel.SetActive(true);
//     }

//     public void OnConnectClicked()
//     {
//         var manager = NetworkManager.Singleton;

//         if (manager == null) return;

//         var transport = (UnityTransport)manager.NetworkConfig.NetworkTransport;
//         transport.SetConnectionData(ipInputField.text, 7777);

//         //Test line for approving spawn
//         manager.ConnectionApprovalCallback = ApproveConnection;

//         manager.StartClient();

//         clientPanel.SetActive(false);
//         lobbyPanel.SetActive(true);
//         //Old code
//         /* var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
//         string ip = ipInputField.text;
//         if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";

//         transport.SetConnectionData(ip, 7777);
//         NetworkManager.Singleton.StartClient();

//         ShowLobby();*/
//     }

//     private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
//     {
//         response.Approved = true;
//         response.CreatePlayerObject = false;
//         response.Position = null;
//         response.Rotation = null;
//     }

//     public void OnBackFromClientConnect()
//     {
//         ShowMainMenu();
//     }

//     public void OnBackFromLobby()
//     {
//         NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
//         NetworkManager.Singleton.Shutdown();
//         ShowMainMenu();
//     }

//     private void ShowMainMenu()
//     {
//         mainMenuPanel.SetActive(true);
//         clientPanel.SetActive(false);
//         lobbyPanel.SetActive(false);
//     }

//     private void ShowLobby()
//     {
//         mainMenuPanel.SetActive(false);
//         clientPanel.SetActive(false);
//         lobbyPanel.SetActive(true);

//         UpdateIPDisplay();
//         RefreshPlayerList();

//         NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//     }

//     private void OnClientConnected(ulong clientId)
//     {
//         RefreshPlayerList();
//     }

//     private void UpdateIPDisplay()
//     {
//         if (ipText == null) return;

//         if (NetworkManager.Singleton.IsHost)
//         {
//             string ip = IPHelper.GetLocalIPv4();
//             ipText.text = "Host IP: " + ip;
//         }
//         else
//         {
//             ipText.text = "Connected to Host";
//         }
//     }

//     private void RefreshPlayerList()
//     {
//         if (playerListContainer == null || playerListEntryPrefab == null) return;


//         foreach (Transform child in playerListContainer.transform)
//         {
//             Destroy(child.gameObject);
//         }

//         foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
//         {
//             //Things commented out in here are being worked on
//             //string playerName = $"Player {client.ClientId}";
//             GameObject entry = Instantiate(playerListEntryPrefab, playerListContainer.transform);
//             //entry.GetComponent<TextMeshProUGUI>().text = playerName;

//             var textComp = entry.GetComponentInChildren<TextMeshProUGUI>();
//             if (textComp != null)
//             {
//                 bool isHost = client.ClientId == NetworkManager.Singleton.LocalClientId && NetworkManager.Singleton.IsHost;
//                 textComp.text = $"Player {client.ClientId}" + (isHost ? " (Host) " : "");
//             }
//         }
//     }
    /* ------------------------------ Networking --------------------------------- */
    using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : NetworkBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject clientPanel;
    public GameObject lobbyPanel;

    [Header("Input")]
    public TMP_InputField ipInputField;

    [Header("LobbyUI")]
    public TextMeshProUGUI ipText;
    public GameObject playerListContainer;
    public GameObject playerListEntryPrefab;

    private void Start()
    {
        if (!IsClient && !IsServer)
        {
            ShowMainMenu();
        }
        else
        {
            ShowLobby();
        }
    }

    public void OnHostClicked()
    {
        var manager = NetworkManager.Singleton;
        if (manager == null) return;

        var transport = (UnityTransport)manager.NetworkConfig.NetworkTransport;
        transport.SetConnectionData("0.0.0.0", 7777);

        manager.StartHost();

        ShowLobby();
    }

    public void OnClientClicked()
    {
        mainMenuPanel.SetActive(false);
        clientPanel.SetActive(true);
    }

    public void OnConnectClicked()
    {
        var manager = NetworkManager.Singleton;
        if (manager == null) return;

        var transport = (UnityTransport)manager.NetworkConfig.NetworkTransport;
        string ip = ipInputField.text;
        if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";
        transport.SetConnectionData(ip, 7777);

        manager.ConnectionApprovalCallback = ApproveConnection;
        manager.StartClient();

        ShowLobby();
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;
    }

    public void OnBackFromClientConnect()
    {
        ShowMainMenu();
    }

    public void OnBackFromLobby()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.Shutdown();
        }

        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        clientPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    private void ShowLobby()
    {
        mainMenuPanel.SetActive(false);
        clientPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        UpdateIPDisplay();
        RefreshPlayerList();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        RefreshPlayerList();
    }

    private void UpdateIPDisplay()
    {
        if (ipText == null) return;

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            string ip = IPHelper.GetLocalIPv4();
            ipText.text = "Host IP: " + ip;
        }
        else
        {
            ipText.text = "Connected to Host";
        }
    }

    private void RefreshPlayerList()
    {
        if (playerListContainer == null || playerListEntryPrefab == null) return;

        foreach (Transform child in playerListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject entry = Instantiate(playerListEntryPrefab, playerListContainer.transform);

            var textComp = entry.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
            {
                bool isHost = client.ClientId == NetworkManager.Singleton.LocalClientId && NetworkManager.Singleton.IsHost;
                textComp.text = $"Player {client.ClientId}" + (isHost ? " (Host)" : "");
            }
        }
    }
}

// }
