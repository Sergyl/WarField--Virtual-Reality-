using UnityEngine;

public class BulletFire : MonoBehaviour
{

	void OnCollisionEnter(Collision collision)
	{
		var hit = collision.gameObject;
		var health = hit.GetComponent<Health>();
		if (health != null)
		{
			health.TakeDamage(45);
		}

		Destroy(gameObject);
	}
}