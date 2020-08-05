using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetCamera : NetworkBehaviour
{
	public Transform CameraSet;
	public GameObject MainCamera;

	// Use this for initialization
	void Start()
	{
		if (!isLocalPlayer)
			return;

		MainCamera.GetComponent<CameraFollow>().SetTarget(CameraSet);
	}
}
