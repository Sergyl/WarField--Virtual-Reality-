using UnityEngine;
using UnityEngine.Networking;


public class GameControl : NetworkBehaviour {

    private bool playerWarned = false;

	[SyncVar]
	private bool result = false;

	public void Update()
    {
		if (IsGameEnded() && !playerWarned)
        {
			WarnToMe();
			playerWarned = true;
		}
    }

    public bool IsGameEnded()
    {
		int playersLose = 0;
		int numberLosers = 0;

		if(GameObject.FindGameObjectWithTag("Player") != null)
		{
			playersLose = GameObject.FindGameObjectsWithTag("Player").Length - 1;
		}

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
			if (playersLose == 0)
			{
				if (player.GetComponent<Health>().GetLive() == 0)
					result = true;
			}
			else
			{
				if (player.GetComponent<Health>().GetLive() == 0)
					numberLosers++;

				if (playersLose != 0 && (numberLosers == playersLose))
					result = true;
			}

			if (player.GetComponent<Chronometer>().GetSecA() == 0 && player.GetComponent<Chronometer>().GetSecB() == 0 && player.GetComponent<Chronometer>().GetMinA() == 0 && player.GetComponent<Chronometer>().GetMinB() == 0)
				result = true;
        }

        return result;
    }

    public void WarnToMe()
	{
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
			player.GetComponent<PlayerCode>().SetFinish(true);
		}
    }

	public bool GetPlayerWarned()
	{
		return playerWarned;
	}

	public bool GetResult()
	{
		return result;
	}
}
