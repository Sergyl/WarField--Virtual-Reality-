using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawn : NetworkBehaviour
{
    public GameObject itemPrefab;
    public int numberOfItems;

    public override void OnStartServer()
    {
        SpawnWave();
        InvokeRepeating("SpawnWave", 30.0f, 35.0f);
    }

    void SpawnWave()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-60.0f, 60.0f), 0.0f, Random.Range(-60.0f, 60.0f));
            var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);

            spawnPosition += transform.position;
            var item = (GameObject)Instantiate(itemPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(item);
        }
    }
}
