using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : NetworkBehaviour
{
    public bool destroyOnDeath;
    public const int maxHealth = 100;
	public RectTransform healthBar;
    public Text numberOfLives;
    public const int maxLive = 5;
    private NetworkStartPosition[] spawnPoints;

	[SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    [SyncVar]
    public int currentLive = maxLive;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        numberOfLives.text = (maxLive.ToString());
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

	public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;

				if (currentLive > 0)
				{
                    RpcTakeAwayLive();

					//Este if se coloca para que funcionen los puntos tambien cuando se usan servidores dedicados o MatchMaker
					if (!isClient)
						TakeAwayLive();

                    RpcRespawn();
				} else
				{
                    RpcRespawn();
                }
            }
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

    public void SetLiveMax()
    {
        if (!isServer)
            return;

        RpcSetLiveMax();
    }

    [ClientRpc]
    void RpcTakeAwayLive()
    {
        currentLive -= 1;
        numberOfLives.text = (currentLive.ToString());
    }

	void TakeAwayLive()
	{
		currentLive -= 1;
		numberOfLives.text = (currentLive.ToString());
	}

	[ClientRpc]
    private void RpcSetLiveMax()
    {
        currentLive = maxLive;
    }

    [ClientRpc]
    void RpcRespawn()
    {
		if (!isLocalPlayer)
			return;

        Vector3 spawnPoint = Vector3.zero;

		if (spawnPoints != null && spawnPoints.Length > 0)
			spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

        transform.position = spawnPoint;
    }

    public int GetMaxLife()
    {
        return maxLive;
    }

    public int GetLive()
    {
        return currentLive;
    }
}