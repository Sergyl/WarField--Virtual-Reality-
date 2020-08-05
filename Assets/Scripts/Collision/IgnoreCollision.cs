using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet")
		{
			Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
		}
	}

}