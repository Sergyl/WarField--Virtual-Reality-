using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnemyPartisan : NetworkBehaviour
{
	private Transform player;
	private float  MoveSpeed = 5.0f;
	private readonly float MinDist = 5.0f;

	void Update()
	{
		if (!isServer)
			return;

		try
		{
			player = GetClosestEnemy(FindNearestTransform());
			transform.LookAt(player.Find("TargetBody").transform);

			if (transform.position.y <= 1.0f)
				transform.position += transform.up;

			if (Vector3.Distance(transform.position, player.position) >= MinDist)
			{
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
		}
		catch (System.NullReferenceException)
		{
			return;
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
		Transform[] enemiesLocation;

		enemies = GameObject.FindGameObjectsWithTag("Player");
		enemiesLocation = new Transform[enemies.Length];

		for (int i = 0; i < enemies.Length; i++)
		{
			enemiesLocation[i] = enemies[i].transform;
		}

		return enemiesLocation;
	}

	void OnCollisionEnter(Collision collision)
	{
		var hit = collision.gameObject;
		if (hit.gameObject.tag == "Player")
		{
			var health = hit.GetComponent<Health>();
			if (health != null)
			{
				health.TakeDamage(5);
			}
		}
	}
}
