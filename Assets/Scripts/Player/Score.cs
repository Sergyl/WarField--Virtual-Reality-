using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Score : NetworkBehaviour {

	public Text PanelScore;

	[SyncVar]
	private int score = 0;

    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

		PanelScore.text = ("Score: " + score.ToString());
	}

	[ClientRpc]
	public void RpcAddScore(int point)
	{
		score += point;
	}

	public void AddScore(int point)
	{
		score += point;
	}

	public int GetScore()
    {
        return score;
    }
}
