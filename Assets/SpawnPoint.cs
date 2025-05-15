/*using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] spawnLocations;
    public GameObject[] player;

    private Vector3 respawnLocations;

    void Awake()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    void Start()
    {
        player = (GameObject)Resources.Load("Player 2", typeof(GameObject));

        respawnLocations = player.transform.position;

        SpawnPlayer();
    }

    
    void Update()
    {
       
    }

    private void SpawnPlayer()
    {
        int spawn = Random.Range(0, spawnLocations.Length);
        GameObject.Instantiate(player, spawnLocations[spawn].transform.position, Quaternion.identity);
    }
}
*/