using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class WinScreen : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWinMessage;

    public void Start()
    {
        txtWinMessage.enabled = false;
    }

    public void DisplayForLoser(ulong client)
    {
        txtWinMessage.enabled = true;
        txtWinMessage.text = $"Player {client} wins!";
    }

    public void DisplayForWinner()
    {
        txtWinMessage.enabled = true;
        txtWinMessage.text = $"YOU WIN!";
    }


}
