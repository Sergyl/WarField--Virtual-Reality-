using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Lives : NetworkBehaviour
{
    public Text life;
    private int vida;

    void Update()
	{
        if (!isLocalPlayer)
            return;

        Health health = GetComponent<Health>();
        vida = health.GetLive();
        life.text = (vida.ToString());
    }
}