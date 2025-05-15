using UnityEngine;
using Unity.Netcode;

public class ScoreTracker : NetworkBehaviour
{
    public class ScoreEntry // class for each player entry in the score list. contains client id and score
    {
        public NetworkVariable<ulong> OwnerClientId = new NetworkVariable<ulong>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );
        
        public NetworkVariable<int> score = new NetworkVariable<int>(
            0,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        
    }
    public int targetScore = 8;
    public static int maxPlayers = 8;
    public NetworkVariable<int> playersInGame = new NetworkVariable<int>(0, default, default);  // tracks number of players in game
    public ScoreEntry[] scoreList = new ScoreEntry[maxPlayers];  // create score list
    public WinScreen winScript;
    public NetworkVariable<bool> isGameOver = new NetworkVariable<bool>(false, default, default);

    

    [Rpc(SendTo.Owner)]
    public void AddPlayerRpc(ulong client)  // add player to list of scores
    {
        if (scoreList[client] != null) return;  // if entry already exists then exit function

        scoreList[client] = new ScoreEntry();  // create new entry with client id and score 0
        scoreList[client].OwnerClientId.Value = client;
        scoreList[client].score.Value = 0;
        playersInGame.Value++;  // increase the count for the number of players in server
        Debug.Log($"SCORE UPDATE: Total Ingame: {playersInGame.Value}, Client: {scoreList[client].OwnerClientId.Value }, Score: {scoreList[client].score.Value}");   
    }

    [Rpc(SendTo.Owner)]
    public void UpdatePlayerScoreRpc(ulong client, int newScore)
    {
        if (scoreList[client].OwnerClientId.Value == client) // find entry of the client
        {
            scoreList[client].score.Value = newScore;   // update their score
            Debug.Log($"SCORE UPDATE: Client: {scoreList[client].OwnerClientId.Value }, NEW Score: {scoreList[client].score.Value}");

            if (scoreList[client].score.Value >= targetScore && (isGameOver.Value == false))    // check if reached winning score total
            {   
                VictoryRpc(client, scoreList[client].score.Value);
                isGameOver.Value = true;
                GameStart.Instance.PauseGameClientRpc();
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    public void VictoryRpc(ulong client, int score)
    {
        Debug.Log($"SCORE UPDATE: WINNER WINNER CHICKEN DINNER!!! client {client}, score {score}");    // test win message
        winScript = GetComponent<WinScreen>();
        if (scoreList[client] == null)
            winScript.DisplayForWinner();
        else
            winScript.DisplayForLoser(client);
    }

}
