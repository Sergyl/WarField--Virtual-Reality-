using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Chronometer : NetworkBehaviour
{
	private int secondsA = 0;
	private int secondsB = 0;
	private int minutesA = 0;
	private int minutesB = 3;
	public Text chronometer;

	// Use this for initialization
	void Start()
	{
		if (isServer)
			StartCoroutine("AddTime");
	}

	// Update is called once per frame
	void Update()
	{
		chronometer.text = (+ minutesA+ ""+ minutesB+ ":" + secondsA +""+ secondsB);

		if (minutesA == 0 && minutesB == 0 && secondsA == 0 && secondsB == 0)
			StopCoroutine("AddTime");
	}

	IEnumerator AddTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			secondsB--;

			if(secondsA == 0 && secondsB < 0)
			{
				secondsB = 9;
				secondsA = 5;
				minutesB -= 1;
			}

			if(secondsA != 0 && secondsB < 0)
			{
				secondsB = 9;
				secondsA -= 1;
			}

			if (minutesA == 0 && minutesB == 0 && secondsA == 0 && secondsB == 0)
			{
				secondsB = 0;
				secondsA = 0;
				minutesB = 0;
				minutesA = 0;
			}

			RpcSetChronometer(secondsA, secondsB, minutesA, minutesB);

			if(!isClient)
				SetChronometer(secondsA, secondsB, minutesA, minutesB);

		}
	}

	[ClientRpc]
	public void RpcSetChronometer(int secA, int secB, int minA, int minB)
	{
		secondsA = secA;
		secondsB = secB;
		minutesA = minA;
		minutesB = minB;
	}

	public void SetChronometer(int secA, int secB, int minA, int minB)
	{
		secondsA = secA;
		secondsB = secB;
		minutesA = minA;
		minutesB = minB;
	}

	public int GetSecA()
	{
		return secondsA;
	}

	public int GetSecB()
	{
		return secondsB;
	}

	public int GetMinA()
	{
		return minutesA;
	}

	public int GetMinB()
	{
		return minutesB;
	}
}
