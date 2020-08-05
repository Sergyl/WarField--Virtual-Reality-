using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
	public GameObject enemyPrefab;
    public int numberOfEnemies;

	public override void OnStartServer()
	{
		SpawnWave();
		InvokeRepeating("SpawnWave", 30.0f, 40.0f);
	}

	private void Update()
	{
		if (GameObject.FindWithTag("GameManager") != null)
		{
			if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameControl>().GetResult())
			{
				DeleteEnemy();
				CancelInvoke();
			}
		}
	}

	void SpawnWave()
	{
		for (int i = 0; i < numberOfEnemies; i++)
		{
			var spawnPosition = new Vector3(
				Random.Range(-40.0f, 40.0f),
				0.0f,
				Random.Range(-40.0f, 40.0f));

			var spawnRotation = Quaternion.Euler(
				0.0f,
				Random.Range(0, 180),
				0.0f);

			spawnPosition += transform.position;
			var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
			NetworkServer.Spawn(enemy);
		}
	}

	void DeleteEnemy()
	{
		if(GameObject.FindWithTag("Enemy") != null)
		{
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				NetworkServer.Destroy(enemy);
			}
		}
	}
}