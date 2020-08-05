using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Profiling;
using Prototype.NetworkLobby;

public class PlayerCode : NetworkBehaviour
{
	#region GAMEOBJECTS
	public GameObject BloodyScreen;
	public GameObject bulletPrefab;
	public GameObject UI;
    public GameObject Win;
    public GameObject Lose;
	public GameObject Crosshair;
	public Text UIWinner;
	public Text Name;
	#endregion

	#region VARIABLES
	public Transform bulletSpawn;
    public RectTransform HealthFromHead;
    public RectTransform HealthForScreen;
    private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	private string nameWinner = "";
	private readonly int[] lifeOfEnemy;
	private int[] scoreOfEnemy;
	private bool finish = false;
	private bool lose = false;
	private readonly float speed = 10.0f;
	private readonly float gravity = 150.0f;
	#endregion

	[SyncVar(hook = "OnChangeName")]
	public string pName = "Player";

	[SyncVar(hook = "OnChangeColor")]
	public Color playerColor = Color.white;

	[SyncVar]
	private bool isSending = false;

	[SyncVar]
	private bool isBusy = false;

	[SyncVar]
	private string date;

	[SyncVar]
	private bool win = false;


	private void Start()
	{
		if (!isLocalPlayer)
			return;

		//Active controller and cursor
		controller = GetComponent<CharacterController>();
		UI.SetActive(true);
	}

	void OnDestroy()
	{
		if (!isLocalPlayer)
			return;
	}

	public override void OnStartServer()
	{
		SetPlayerName();
		SetPlayerColor();
	}

	public override void OnStartClient()
	{
		SetPlayerName();
		SetPlayerColor();
	}

	void Update()
	{
		if (!isLocalPlayer)
			return;

		if ((lose || win) && Crosshair.activeSelf)
			Crosshair.SetActive(false);

		//Update end of game
		CheckFinish(finish);

		//Update Canvas for end game
		PutLoseOrWinScreen();

		//Update Health to UI and Screen Damage
		if (HealthForScreen.sizeDelta != HealthFromHead.sizeDelta)
			BloodyScreen.GetComponent<Image>().enabled = true;
		else
			BloodyScreen.GetComponent<Image>().enabled = false;

		HealthForScreen.sizeDelta = HealthFromHead.sizeDelta;



		if (!lose)
		{
			//Code for moving avatar
			moveDirection = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
			transform.Translate(Vector3.forward * speed * moveDirection.y * Time.deltaTime);
			transform.Translate(Vector3.right * speed * moveDirection.x * Time.deltaTime);
		}


		//Code for shooting in PC
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
		{
			CmdFire();
		}

		//Code for going to menu
		if (OVRInput.GetDown(OVRInput.Button.Two))
		{
			if (GameObject.Find("LobbyManager") != null)
			{
				GameObject.Find("LobbyManager").GetComponent<LobbyManager>().GoBackButton();
			}
		}

		//Code for gravity
		moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
		controller.Move(moveDirection * Time.deltaTime);
	}

	public void CheckFinish(bool finish)
	{
		if (finish)
		{
			if (GetComponent<Health>().GetLive() != 0)
			{
				if (MaxScoreOfLifePlayer() == GetComponent<Score>().GetScore())
					CmdIsWin();
				else
					lose = true;
			} else
			{
				lose = true;
			}

			if(lose && nameWinner == "")
			{
				foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
				{
					if (player.GetComponent<PlayerCode>().GetWin())
						nameWinner = player.GetComponent<PlayerCode>().GetName();
				}

				if (nameWinner != "")
					UIWinner.text = ("Ganador: " + nameWinner);
				else
					UIWinner.text = ("No hay ganador");

			}
		}

		if(!finish)
		{
			if (GetComponent<Health>().GetLive() == 0)
				lose = true;
		}
	}

    public int MaxScoreOfLifePlayer()
    {
        scoreOfEnemy = new int[GameObject.FindGameObjectsWithTag("Player").Length];
        int i = 0;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
			if (player.GetComponent<Health>().GetLive() != 0)
			{
				scoreOfEnemy[i] = player.GetComponent<Score>().GetScore();
				i++;
			}
        }

        return Mathf.Max(scoreOfEnemy);
    }

	public void PutLoseOrWinScreen()
	{
		if (win)
			Win.SetActive(true);
		else
			Win.SetActive(false);

		if (lose)
			Lose.SetActive(true);
		else
			Lose.SetActive(false);

		if (lose)
			transform.Find("TargetBody").tag = "Untagged";
	}

	public void OnChangeName(string newName)
	{
		pName = newName;
		SetPlayerName();
	}

	public void SetPlayerName()
	{
		Name.text = (pName.ToString());
	}

	public void OnChangeColor(Color newColor)
	{
		playerColor = newColor;
		SetPlayerColor();
	}

	public void SetPlayerColor()
	{
		foreach (Renderer r in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			r.material.color = playerColor;
		}
	}

	[Command]
	public void CmdIsWin()
	{
		win = true;
	}

	[Command]
	public void CmdIsSending()
	{
		isSending = true;
	}

	[Command]
	public void CmdNotSending()
	{
		isSending = false;
	}

	[Command]
	public void CmdIsBusy()
	{
		isBusy = true;
	}

	[Command]
	public void CmdNotBusy()
	{
		isBusy = false;
	}

	[Command]
    public void CmdFire()
    {
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 150;

		//Identify bullet
		bullet.name = pName;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}

	public void SetWin(bool win)
	{
		this.win = win;
	}

	public void SetLose(bool lose)
	{
		this.lose = lose;
	}

	public void SetFinish(bool finish)
	{
		this.finish = finish;
	}

	public void SetDate(string date)
	{
		this.date = date;
	}

	public bool GetWin()
	{
		return win;
	}

	public bool GetFinish()
	{
		return finish;
	}

	public string GetName()
	{
		return pName;
	}

	public bool IsSending()
	{
		return isSending;
	}

	public bool IsBusy()
	{
		return isBusy;
	}

	public string GetDate()
	{
		return date;
	}
}