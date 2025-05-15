using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


public class LobbyUIManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject playerListContainer;
    public GameObject playerEntryPrefab;

    [Header("Buttons")]
    public Button readyButton;
    public Button startGameButton;
    private LobbyManager lobbyManager;

    private void Start()
    {
        // Get lobby manager game object on start
        lobbyManager = FindFirstObjectByType<LobbyManager>();

        // call update ui function whenever network list change event happens
        lobbyManager.playerNames.OnListChanged += (change) => UpdateUI();
        lobbyManager.readyStates.OnListChanged += (change) => UpdateUI();

        // button onclick events
        readyButton.onClick.AddListener(OnReadyPressed);
        startGameButton.onClick.AddListener(OnStartGamePressed);
    }

    void UpdateUI()
    {
        // edge cases for when lists aren't synced
        if (lobbyManager.playerNames.Count == 0 || lobbyManager.readyStates.Count == 0 || lobbyManager.playerNames.Count != lobbyManager.readyStates.Count) return;

        // during an update delete and remake ui
        foreach (Transform child in playerListContainer.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < lobbyManager.playerNames.Count; i++)
        {
            // create entry and set parent to container
            var entry = Instantiate(playerEntryPrefab, playerListContainer.transform);

            // fill entry with player name and ready status text
            entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = lobbyManager.playerNames[i].ToString();
            entry.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().fontSize = 30;
            // Optionally adjust position (e.g., move closer to name text)
            RectTransform statusRect = entry.transform.Find("PlayerNameText").GetComponent<RectTransform>();
            statusRect.anchoredPosition = new Vector2(75f, 0f); //

            entry.transform.Find("ReadyStatusText").GetComponent<TextMeshProUGUI>().text = lobbyManager.readyStates[i] ? "Ready" : "Not Ready";
            entry.transform.Find("ReadyStatusText").GetComponent<TextMeshProUGUI>().fontSize = 30;
        }

        // set start button active if player is host and all players ready
        startGameButton.gameObject.SetActive(NetworkManager.Singleton.IsHost && lobbyManager.AllPlayersReady());
    }

    void OnReadyPressed()
    {
        // call serverrpc function to set the ready state for client
        lobbyManager.SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    void OnStartGamePressed()
    {
        // when all players ready - load the game scene
        if (NetworkManager.Singleton.IsHost && lobbyManager.AllPlayersReady())
        {
            NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
