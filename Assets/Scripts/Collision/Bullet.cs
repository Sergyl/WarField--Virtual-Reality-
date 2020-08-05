using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Bullet : NetworkBehaviour
{
    private GameObject playerCreator;

    private void Start()
    {
		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (player.GetComponent<PlayerCode>().GetName() == gameObject.name)
				playerCreator = player;
		}
	}

    void OnCollisionEnter(Collision collision)
	{
		if (!isServer)
			return;

        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();

		if (health != null && health.CompareTag("Enemy"))
		{
			health.TakeDamage(25);
			playerCreator.GetComponent<Score>().RpcAddScore(10);

			//Este if se coloca para que funcionen los puntos tambien cuando se usan servidores dedicados o MatchMaker
			if (!isClient)
				playerCreator.GetComponent<Score>().AddScore(10);
		}
		else if (health != null && health.CompareTag("Player"))
		{
			health.TakeDamage(5);
		}

        Destroy(gameObject);
    }
}