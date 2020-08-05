using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyShoot : NetworkBehaviour
{

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	private Transform player;
	private readonly float MoveSpeed = 5.0f;
	private readonly float MaxDist = 75.0f;
	private readonly float MinDist = 5.0f;
	private bool onRange = false;

	void Start()
	{
		InvokeRepeating("Shoot", 2.0f, 5.0f);
	}

	void Update()
	{
		if (!isServer)
			return;

		try
		{
			player = GetClosestEnemy(FindNearestTransform());
			transform.LookAt(player.Find("TargetBody").transform);

			if (Vector3.Distance(transform.position, player.position) >= MinDist)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;

				if (Vector3.Distance(transform.position, player.position) <= MaxDist)
				{
					onRange = true;
				}
			}
		}
		catch (System.NullReferenceException)
		{
			return;
		}
	}

	void Shoot()
	{
		if (onRange)
		{
			Fire();
		}
	}

	Transform GetClosestEnemy(Transform[] enemies)
	{
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach (Transform t in enemies)
		{
			float dist = Vector3.Distance(t.position, currentPos);
			if (dist < minDist)
			{
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}


	public Transform[] FindNearestTransform()
	{
		GameObject[] enemies;
		Transform [] enemiesLocation;

		enemies = GameObject.FindGameObjectsWithTag("Player");
		enemiesLocation = new Transform[enemies.Length];

		for (int i = 0; i < enemies.Length; i++)
		{
			enemiesLocation[i] = enemies[i].transform;
		}

		return enemiesLocation;
	}

	void Fire()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 75;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 6.0f);
	}
}