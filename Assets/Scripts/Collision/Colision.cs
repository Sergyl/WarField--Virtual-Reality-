using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour {

	void OnCollisionStay(Collision collision)
	{
		var hit = collision.gameObject;
		if (hit.CompareTag("Player"))
		{
			var health = hit.GetComponent<Health>();
			if (health != null)
			{
				health.TakeDamage(1);
			}
		}

	}
}
