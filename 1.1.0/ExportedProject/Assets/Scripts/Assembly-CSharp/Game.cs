using System;
using System.Linq;
using DiscordRPC;
using UnityEngine;

public class Game : MonoBehaviour
{
	public float spawnRate = 1f;

	private float nextSpawnTime;

	private bool gameRunning = true;

	private int score;

	private int highscore;

	private float boostLeft;

	private float slownessLeft;

	public float screenWidth;

	public DiscordRpcClient client = new DiscordRpcClient("1216934858182361148");

	public DateTime startTimestamp = DateTime.UtcNow;

	public RuntimePlatform[] desktopPlatforms = new RuntimePlatform[6]
	{
		RuntimePlatform.WindowsPlayer,
		RuntimePlatform.LinuxPlayer,
		RuntimePlatform.OSXPlayer,
		RuntimePlatform.WindowsEditor,
		RuntimePlatform.LinuxEditor,
		RuntimePlatform.OSXEditor
	};

	private void Awake()
	{
		if (PlayerPrefs.HasKey("HighScore"))
		{
			highscore = PlayerPrefs.GetInt("HighScore");
		}
	}

	private void Start()
	{
		screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		GameObject.Find("HighScoreText").GetComponent<TextMesh>().text = $"High Score: {highscore}";
		if (desktopPlatforms.Contains(Application.platform))
		{
			client.Initialize();
			client.SetPresence(new RichPresence
			{
				State = "Playing Foodie Dash",
				Details = $"Score: 0, High Score: {highscore}",
				Assets = new Assets
				{
					LargeImageKey = "https://foodiedash.xytriza.com/assets/bird.png",
					LargeImageText = "Foodie Dash",
					SmallImageKey = "https://xytriza.com/assets/icon.png",
					SmallImageText = "Made by Xytriza!"
				},
				Timestamps = new Timestamps
				{
					Start = startTimestamp
				},
				Buttons = new Button[2]
				{
					new Button
					{
						Label = "Play Foodie Dash in browser",
						Url = "https://foodiedash.xytriza.com/browser"
					},
					new Button
					{
						Label = "Download Foodie Dash",
						Url = "https://foodiedash.xytriza.com/download"
					}
				}
			});
		}
		if (Application.isMobilePlatform)
		{
			GameObject gameObject = new GameObject("LeftArrow");
			GameObject gameObject2 = new GameObject("RightArrow");
			GameObject obj = new GameObject("RestartButton");
			gameObject.AddComponent<SpriteRenderer>();
			gameObject2.AddComponent<SpriteRenderer>();
			obj.AddComponent<SpriteRenderer>();
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("left");
			gameObject2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("right");
			obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("restart");
			gameObject.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
			gameObject2.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
			obj.transform.position = new Vector3(screenWidth / 2.17f, Camera.main.orthographicSize - 1f, 0f);
			gameObject.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
			gameObject2.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
			obj.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
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
		float num2 = 0.18f * (num / 20.19257f);
		float num3 = num2;
		if (boostLeft > 0f)
		{
			num3 = num2 * 1.39f;
		}
		else if (slownessLeft > 0f)
		{
			num3 = num2 * 0.56f;
		}
		if (!Application.isMobilePlatform)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				gameObject.transform.position += new Vector3(0f - num3, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				gameObject.transform.position += new Vector3(num3, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
			}
			if (Input.GetKey(KeyCode.R))
			{
				gameObject.transform.position = new Vector3(0f, -4.5f, 0f);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				slownessLeft = 0f;
				UpdateScore(score);
				GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
				GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
				GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
				GameObject[] array4 = GameObject.FindGameObjectsWithTag("SlowBerry");
				GameObject[] array5 = array;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array2;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array3;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array4;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
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
				gameObject.transform.position += new Vector3(0f - num3, 0f, 0f);
				ClampPosition(num, gameObject);
				gameObject.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point))
			{
				gameObject.transform.position += new Vector3(num3, 0f, 0f);
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
			slownessLeft = 0f;
			UpdateScore(score);
			GameObject[] array6 = GameObject.FindGameObjectsWithTag("Berry");
			GameObject[] array7 = GameObject.FindGameObjectsWithTag("PoisonBerry");
			GameObject[] array8 = GameObject.FindGameObjectsWithTag("UltraBerry");
			GameObject[] array9 = GameObject.FindGameObjectsWithTag("SlowBerry");
			GameObject[] array5 = array6;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array7;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array8;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array9;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
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
		GameObject gameObject = GameObject.Find("BoostText");
		if (boostLeft > 0f)
		{
			boostLeft -= Time.deltaTime;
			gameObject.GetComponent<TextMesh>().text = "Boost expires in " + $"{boostLeft:0.0}" + "s";
		}
		else if (slownessLeft > 0f)
		{
			slownessLeft -= Time.deltaTime;
			gameObject.GetComponent<TextMesh>().text = "Slowness expires in " + $"{slownessLeft:0.0}" + "s";
		}
		else
		{
			gameObject.GetComponent<TextMesh>().text = "";
		}
		if (desktopPlatforms.Contains(Application.platform) && Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void SpawnBerries()
	{
		if (Time.time >= nextSpawnTime && gameRunning)
		{
			nextSpawnTime = Time.time + 1f / spawnRate;
			float value = UnityEngine.Random.value;
			GameObject gameObject;
			if (value <= 0.6f)
			{
				gameObject = new GameObject("Berry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Berry");
				gameObject.tag = "Berry";
			}
			else if (value <= 0.8f)
			{
				gameObject = new GameObject("PoisonBerry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PoisonBerry");
				gameObject.tag = "PoisonBerry";
			}
			else if (value <= 0.9f)
			{
				gameObject = new GameObject("SlowBerry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("SlowBerry");
				gameObject.tag = "SlowBerry";
			}
			else
			{
				gameObject = new GameObject("UltraBerry");
				gameObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UltraBerry");
				gameObject.tag = "UltraBerry";
			}
			float num = Camera.main.orthographicSize * 2f * Camera.main.aspect;
			float x = UnityEngine.Random.Range((0f - num) / 2.17f, num / 2.17f);
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
			ClampPosition(screenWidth, GameObject.Find("Bird"));
			if (Application.isMobilePlatform)
			{
				GameObject obj = GameObject.Find("LeftArrow");
				GameObject gameObject = GameObject.Find("RightArrow");
				GameObject gameObject2 = GameObject.Find("RestartButton");
				obj.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
				gameObject.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
				gameObject2.transform.position = new Vector3(screenWidth / 2.17f, Camera.main.orthographicSize - 1f, 0f);
				obj.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
				gameObject.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
				gameObject2.transform.localScale = new Vector3(screenWidth / 20.19257f, screenWidth / 20.19257f, 1f);
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("SlowBerry");
		GameObject gameObject3 = GameObject.Find("Bird");
		GameObject[] array5 = array;
		foreach (GameObject gameObject4 in array5)
		{
			if (gameObject4.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject4);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject4.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("eat"), Camera.main.transform.position);
				UnityEngine.Object.Destroy(gameObject4);
				score++;
				UpdateScore(score);
			}
		}
		array5 = array2;
		foreach (GameObject gameObject5 in array5)
		{
			if (gameObject5.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject5);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject5.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("death"), Camera.main.transform.position);
				GameObject[] array6 = array;
				for (int j = 0; j < array6.Length; j++)
				{
					UnityEngine.Object.Destroy(array6[j]);
				}
				array6 = array2;
				for (int j = 0; j < array6.Length; j++)
				{
					UnityEngine.Object.Destroy(array6[j]);
				}
				array6 = array3;
				for (int j = 0; j < array6.Length; j++)
				{
					UnityEngine.Object.Destroy(array6[j]);
				}
				array6 = array4;
				for (int j = 0; j < array6.Length; j++)
				{
					UnityEngine.Object.Destroy(array6[j]);
				}
				gameObject3.transform.position = new Vector3(0f, -4.5f, 0f);
				gameObject3.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				slownessLeft = 0f;
				UpdateScore(score);
			}
		}
		array5 = array3;
		foreach (GameObject gameObject6 in array5)
		{
			if (gameObject6.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject6);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject6.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("powerup"), Camera.main.transform.position, 0.5f);
				UnityEngine.Object.Destroy(gameObject6);
				if (slownessLeft > 0f)
				{
					slownessLeft = 0f;
					score++;
					UpdateScore(score);
				}
				else
				{
					boostLeft += 10f;
					score += 5;
					UpdateScore(score);
				}
			}
		}
		array5 = array4;
		foreach (GameObject gameObject7 in array5)
		{
			if (gameObject7.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject7);
			}
			else if (Vector3.Distance(gameObject3.transform.position, gameObject7.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("slowness"), Camera.main.transform.position, 0.5f);
				UnityEngine.Object.Destroy(gameObject7);
				boostLeft = 0f;
				slownessLeft = 10f;
				if (score > 0)
				{
					score--;
					UpdateScore(score);
				}
			}
		}
	}

	private void UpdateScore(int score)
	{
		GameObject obj = GameObject.Find("ScoreText");
		GameObject gameObject = GameObject.Find("HighScoreText");
		if (score > highscore)
		{
			highscore = score;
		}
		PlayerPrefs.SetInt("HighScore", highscore);
		PlayerPrefs.Save();
		obj.GetComponent<TextMesh>().text = "Score: " + score;
		gameObject.GetComponent<TextMesh>().text = "High Score: " + highscore;
		if (desktopPlatforms.Contains(Application.platform))
		{
			client.SetPresence(new RichPresence
			{
				Details = "Playing Foodie Dash",
				State = $"Score: {score}, High Score: {highscore}",
				Assets = new Assets
				{
					LargeImageKey = "https://foodiedash.xytriza.com/assets/bird.png",
					LargeImageText = "Foodie Dash",
					SmallImageKey = "https://xytriza.com/assets/icon.png",
					SmallImageText = "Made by Xytriza!"
				},
				Timestamps = new Timestamps
				{
					Start = startTimestamp
				},
				Buttons = new Button[2]
				{
					new Button
					{
						Label = "Play Foodie Dash in browser",
						Url = "https://foodiedash.xytriza.com/browser"
					},
					new Button
					{
						Label = "Download Foodie Dash",
						Url = "https://foodiedash.xytriza.com/download"
					}
				}
			});
		}
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("HighScore", highscore);
		PlayerPrefs.Save();
		if (desktopPlatforms.Contains(Application.platform))
		{
			client.Dispose();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (desktopPlatforms.Contains(Application.platform))
		{
			client.SetPresence(new RichPresence
			{
				Details = "Idle",
				State = $"Score: {score}, High Score: {highscore}",
				Assets = new Assets
				{
					LargeImageKey = "https://foodiedash.xytriza.com/assets/bird.png",
					LargeImageText = "Foodie Dash",
					SmallImageKey = "https://xytriza.com/assets/icon.png",
					SmallImageText = "Made by Xytriza!"
				},
				Buttons = new Button[2]
				{
					new Button
					{
						Label = "Play Foodie Dash in browser",
						Url = "https://foodiedash.xytriza.com/browser"
					},
					new Button
					{
						Label = "Download Foodie Dash",
						Url = "https://foodiedash.xytriza.com/download"
					}
				}
			});
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (desktopPlatforms.Contains(Application.platform))
		{
			client.SetPresence(new RichPresence
			{
				Details = "Playing Foodie Dash",
				State = $"Score: {score}, High Score: {highscore}",
				Assets = new Assets
				{
					LargeImageKey = "https://foodiedash.xytriza.com/assets/bird.png",
					LargeImageText = "Foodie Dash",
					SmallImageKey = "https://xytriza.com/assets/icon.png",
					SmallImageText = "Made by Xytriza!"
				},
				Timestamps = new Timestamps
				{
					Start = startTimestamp
				},
				Buttons = new Button[2]
				{
					new Button
					{
						Label = "Play Foodie Dash in browser",
						Url = "https://foodiedash.xytriza.com/browser"
					},
					new Button
					{
						Label = "Download Foodie Dash",
						Url = "https://foodiedash.xytriza.com/download"
					}
				}
			});
		}
	}
}
