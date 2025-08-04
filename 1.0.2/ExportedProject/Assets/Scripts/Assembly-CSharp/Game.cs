using DiscordRPC;
using UnityEngine;

public class Game : MonoBehaviour
{
	public float spawnRate = 1f;

	private float nextSpawnTime;

	private bool gameRunning = true;

	private int score;

	private float boostLeft;

	public float screenWidth;

	private void Start()
	{
		screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		GameObject gameObject = new GameObject("Berry");
		GameObject obj = new GameObject("Bird");
		Resources.Load<AudioClip>("death");
		Resources.Load<AudioClip>("eat");
		GameObject gameObject2 = new GameObject("PoisionBerry");
		GameObject gameObject3 = new GameObject("UltraBerry");
		gameObject.AddComponent<SpriteRenderer>();
		obj.AddComponent<SpriteRenderer>();
		obj.AddComponent<Rigidbody2D>();
		obj.GetComponent<Rigidbody2D>().gravityScale = 0f;
		gameObject2.AddComponent<SpriteRenderer>();
		gameObject3.AddComponent<SpriteRenderer>();
		obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
		obj.transform.position = new Vector3(0f, -4.5f, 0f);
		obj.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
		obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("bird");
		GameObject obj2 = new GameObject("ScoreText");
		obj2.AddComponent<TextMesh>();
		obj2.GetComponent<TextMesh>().text = "Score: 0";
		obj2.GetComponent<TextMesh>().characterSize = 0.1f;
		obj2.GetComponent<TextMesh>().fontSize = 100;
		obj2.GetComponent<TextMesh>().color = Color.white;
		obj2.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
		obj2.transform.position = new Vector3(0f, 4f, 0f);
		GameObject obj3 = new GameObject("BoostText");
		obj3.AddComponent<TextMesh>();
		obj3.GetComponent<TextMesh>().text = "";
		obj3.GetComponent<TextMesh>().characterSize = 0.1f;
		obj3.GetComponent<TextMesh>().fontSize = 50;
		obj3.GetComponent<TextMesh>().color = Color.white;
		obj3.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
		obj3.transform.position = new Vector3(0f, 3f, 0f);
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			DiscordRpcClient discordRpcClient = new DiscordRpcClient("1216934858182361148");
			discordRpcClient.Initialize();
			discordRpcClient.SetPresence(new RichPresence
			{
				Details = "Playing Foodie Dash",
				State = "Eating Berries",
				Assets = new Assets
				{
					LargeImageKey = "https://foodiedash.xytriza.com/assets/bird.png",
					LargeImageText = "Foodie Dash",
					SmallImageKey = "https://xytriza.com/assets/icon.png",
					SmallImageText = "Made by Xytriza!"
				},
				Timestamps = Timestamps.Now
			});
		}
		if (Application.isMobilePlatform)
		{
			GameObject gameObject4 = new GameObject("LeftArrow");
			GameObject gameObject5 = new GameObject("RightArrow");
			GameObject obj4 = new GameObject("RestartButton");
			gameObject4.AddComponent<SpriteRenderer>();
			gameObject5.AddComponent<SpriteRenderer>();
			obj4.AddComponent<SpriteRenderer>();
			gameObject4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("left");
			gameObject5.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("right");
			obj4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("restart");
			gameObject4.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
			gameObject5.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
			obj4.transform.position = new Vector3(screenWidth / 2.17f, Camera.main.orthographicSize - 1f, 0f);
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
	}

	private void MoveBird()
	{
		if (!gameRunning)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("Bird");
		float num = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		float num2 = 0.18f;
		if (boostLeft > 0f)
		{
			num2 = 0.25f + 0.002f * (float)(int)boostLeft;
		}
		if (!Application.isMobilePlatform)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				gameObject.transform.position += new Vector3(0f - num2, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				gameObject.transform.position += new Vector3(num2, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
			}
			if (Input.GetKey(KeyCode.R))
			{
				gameObject.transform.position = new Vector3(0f, -4.5f, 0f);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				UpdateScore(score);
				GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
				GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
				GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
				GameObject[] array4 = array;
				for (int i = 0; i < array4.Length; i++)
				{
					Object.Destroy(array4[i]);
				}
				array4 = array2;
				for (int i = 0; i < array4.Length; i++)
				{
					Object.Destroy(array4[i]);
				}
				array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					Object.Destroy(array4[i]);
				}
			}
			return;
		}
		GameObject gameObject2 = GameObject.Find("LeftArrow");
		GameObject gameObject3 = GameObject.Find("RightArrow");
		GameObject gameObject4 = GameObject.Find("RestartButton");
		if (Input.touchCount <= 0)
		{
			return;
		}
		Touch touch = Input.GetTouch(0);
		Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
		point.z = 0f;
		if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
		{
			if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point))
			{
				gameObject.transform.position += new Vector3(0f - num2, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point))
			{
				gameObject.transform.position += new Vector3(num2, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
			}
		}
		if (touch.phase == TouchPhase.Began && gameObject4.GetComponent<SpriteRenderer>().bounds.Contains(point))
		{
			gameObject.transform.position = new Vector3(0f, -4.5f, 0f);
			gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			score = 0;
			boostLeft = 0f;
			UpdateScore(score);
			GameObject[] array5 = GameObject.FindGameObjectsWithTag("Berry");
			GameObject[] array6 = GameObject.FindGameObjectsWithTag("PoisonBerry");
			GameObject[] array7 = GameObject.FindGameObjectsWithTag("UltraBerry");
			GameObject[] array4 = array5;
			for (int i = 0; i < array4.Length; i++)
			{
				Object.Destroy(array4[i]);
			}
			array4 = array6;
			for (int i = 0; i < array4.Length; i++)
			{
				Object.Destroy(array4[i]);
			}
			array4 = array7;
			for (int i = 0; i < array4.Length; i++)
			{
				Object.Destroy(array4[i]);
			}
		}
	}

	private void ClampPosition(float screenWidth, GameObject bird)
	{
		float num = screenWidth / 2.17f;
		float x = Mathf.Clamp(bird.transform.position.x, 0f - num, num);
		bird.transform.position = new Vector3(x, bird.transform.position.y, bird.transform.position.z);
	}

	private void FixedUpdate()
	{
		MoveBird();
		SpawnBerries();
		if (boostLeft > 0f)
		{
			boostLeft -= Time.deltaTime;
			GameObject.Find("BoostText").GetComponent<TextMesh>().text = "Boost expires in " + $"{boostLeft:0.00}" + "s";
		}
		else
		{
			GameObject.Find("BoostText").GetComponent<TextMesh>().text = "";
		}
		if (Application.platform == RuntimePlatform.WindowsPlayer && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void SpawnBerries()
	{
		if (Time.time >= nextSpawnTime && gameRunning)
		{
			nextSpawnTime = Time.time + 1f / spawnRate;
			float value = Random.value;
			GameObject gameObject;
			if (value <= 0.9f)
			{
				gameObject = new GameObject("Berry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Berry");
				gameObject.tag = "Berry";
			}
			else if (value <= 0.7f)
			{
				gameObject = new GameObject("PoisonBerry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PoisonBerry");
				gameObject.tag = "PoisonBerry";
			}
			else
			{
				gameObject = new GameObject("UltraBerry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UltraBerry");
				gameObject.tag = "UltraBerry";
			}
			float num = Camera.main.orthographicSize * 2f * Camera.main.aspect;
			float x = Random.Range((0f - num) / 2.17f, num / 2.17f);
			gameObject.transform.position = new Vector3(x, Camera.main.orthographicSize + 1f, 0f);
			Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.gravityScale = 0f;
			rigidbody2D.velocity = new Vector2(0f, -3f);
		}
	}

	private void Update()
	{
		if (screenWidth != Camera.main.orthographicSize * 2f * Camera.main.aspect)
		{
			screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
			if (Application.isMobilePlatform)
			{
				GameObject obj = GameObject.Find("LeftArrow");
				GameObject gameObject = GameObject.Find("RightArrow");
				GameObject gameObject2 = GameObject.Find("RestartButton");
				obj.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
				gameObject.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
				gameObject2.transform.position = new Vector3(screenWidth / 2.17f, Camera.main.orthographicSize - 1f, 0f);
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
		GameObject gameObject3 = GameObject.Find("Bird");
		GameObject[] array4 = array;
		foreach (GameObject gameObject4 in array4)
		{
			if (gameObject4.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				Object.Destroy(gameObject4);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject4.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("eat"), Camera.main.transform.position);
				Object.Destroy(gameObject4);
				score++;
				UpdateScore(score);
			}
		}
		array4 = array2;
		foreach (GameObject gameObject5 in array4)
		{
			if (gameObject5.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				Object.Destroy(gameObject5);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject5.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("death"), Camera.main.transform.position);
				GameObject[] array5 = array;
				for (int j = 0; j < array5.Length; j++)
				{
					Object.Destroy(array5[j]);
				}
				array5 = array2;
				for (int j = 0; j < array5.Length; j++)
				{
					Object.Destroy(array5[j]);
				}
				array5 = array3;
				for (int j = 0; j < array5.Length; j++)
				{
					Object.Destroy(array5[j]);
				}
				gameObject3.transform.position = new Vector3(0f, -4.5f, 0f);
				gameObject3.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				UpdateScore(score);
			}
		}
		array4 = array3;
		foreach (GameObject gameObject6 in array4)
		{
			if (gameObject6.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				Object.Destroy(gameObject6);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject6.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("eat"), Camera.main.transform.position);
				Object.Destroy(gameObject6);
				score += 5;
				UpdateScore(score);
				boostLeft += 10f;
			}
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
	}

	private void UpdateScore(int score)
	{
		GameObject.Find("ScoreText").GetComponent<TextMesh>().text = "Score: " + score;
	}
}
