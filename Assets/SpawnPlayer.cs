/*using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject spawnLocation;
    public GameObject player;
    private Vector3 respawnLocation;

    void Start()
    {
        player = (GameObject)Resources.Load("Player2", typeof(GameObject));

        spawnLocation = GameObject.FindGameObjectsWithTag("SpawnPoint");

        respawnLocation = player.transform.position;

        SpawnCharacter();
    }

    
    void Update()
    {
        
    }

    private void SpawnCharacter()
    {
        GameObject.Instantiate(player, spawnLocation.transform.position, Quaternion.identity);
    }
}
*/