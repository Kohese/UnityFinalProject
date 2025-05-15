using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System;
using System.Net;
using AddressFamily = System.Net.Sockets.AddressFamily;
using TMPro;
using System.Collections;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;

// when a player leaves - client id is not reused in the same session
public class LobbyManager : NetworkBehaviour
{
    // create network lists to store player names and ready states
    public NetworkList<FixedString32Bytes> playerNames;
    public NetworkList<bool> readyStates;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject clientPanel;
    public GameObject lobbyPanel;
    [SerializeField] public GameObject hostBtn;

    
    [SerializeField] public TextMeshProUGUI ipText;

    [Header("Input")]
    public TMP_InputField ipInputField;

    private void Awake()
    {
        playerNames = new NetworkList<FixedString32Bytes>();
        readyStates = new NetworkList<bool>();
        ShowMainMenu();
        hostBtn.SetActive(true);
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        clientPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        hostBtn.SetActive(true);
        Debug.Log("HERE ISM A");
    }


    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            // when clients connect/disconnect run the function
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }


    public void OnHostClicked()
    {
        var manager = NetworkManager.Singleton;

        if (manager == null) return;

        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is null!");
            return;
        }

        var transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.SetConnectionData("0.0.0.0", 7777);

        //Test lines to see about approving spawn
        // manager.NetworkConfig.ConnectionApproval = true;
        // manager.ConnectionApprovalCallback = ApproveConnection;

        manager.StartHost();
        mainMenuPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        UpdateIPDisplay();
    }

    public void StartClient()
    {
        // use this when creating textbox to enter ip
        string ip = ipInputField.text;

        UnityTransport transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

        // set ip to string so player can input the ip
        // transport.SetConnectionData(ip, 7777);

        // need to change from 127.0.0.1 once testing is done
        transport.SetConnectionData(ip, 7777);
        NetworkManager.Singleton.StartClient();
    }

    // when client connects add name and ready state to network list
    private void OnClientConnected(ulong clientId)
    {
        if (!IsServer || !IsHost) return;

        playerNames.Add($"Player {clientId}");
        readyStates.Add(false);
        ShowLobbyClientRpc();
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

    public void OnBackFromLobby()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.Shutdown();
        }

        ShowMainMenu();
    }

    public void OnClientClicked()
    {
        mainMenuPanel.SetActive(false);
        clientPanel.SetActive(true);
    }

    [ClientRpc]
    public void ShowLobbyClientRpc(ClientRpcParams rpcParams = default)
    {
        mainMenuPanel.SetActive(false);
        clientPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    // when a client disconnects clear and add remaining clients to network lists
    private void OnClientDisconnected(ulong clientId)
    {
        if (!IsServer || !IsHost) return;
        RebuildLists();
    }

    private void RebuildLists()
    {
        playerNames.Clear();
        readyStates.Clear();
        foreach (var client in NetworkManager.Singleton.ConnectedClientsIds)
        {
            playerNames.Add($"Player {client}");
            readyStates.Add(false);
        }
    }

    // use serverrpc when client need to share or sync information

    // use serverrpc to store the ready state of clients by client id
    [ServerRpc(RequireOwnership = false)]
    public void SetReadyServerRpc(ulong clientId)
    {
        int index = GetClientIndex(clientId);
        if (index >= 0 && index < readyStates.Count)
        {
            readyStates[index] = !readyStates[index];
        }
    }

    // function that returns true if every client is ready
    public bool AllPlayersReady()
    {
        foreach (var ready in readyStates)
        {
            if (!ready) return false;
        }
        return true;
    }

    // use client id to get the index of the client for network lists
    public int GetClientIndex(ulong clientId)
    {
        int i = 0;
        foreach (var id in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (id == clientId)
                return i;
            i++;
        }
        return -1;
    }
}
