using System;
using System.Linq;
using DiscordRPC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public float spawnRate = 1f;

	private float nextSpawnTime;

	private int score;

	private int highscore;

	private float boostLeft;

	private float slownessLeft;

	private float screenWidth;

	private DiscordRpcClient client = new DiscordRpcClient("1216934858182361148");

	private DateTime startTimestamp = DateTime.UtcNow;

	private RuntimePlatform[] desktopPlatforms = new RuntimePlatform[6]
	{
		RuntimePlatform.WindowsPlayer,
		RuntimePlatform.LinuxPlayer,
		RuntimePlatform.OSXPlayer,
		RuntimePlatform.WindowsEditor,
		RuntimePlatform.LinuxEditor,
		RuntimePlatform.OSXEditor
	};

	private bool isGrounded;

	private float jumpForce = 9f;

	private float groundYPosition = -4.3f;

	private GameObject bird;

	private Rigidbody2D rb;

	private void Awake()
	{
		bird = GameObject.Find("Bird");
		rb = bird.GetComponent<Rigidbody2D>();
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
		if (PlayerPrefs.GetInt("Setting2") == 1)
		{
			GameObject gameObject = new GameObject("LeftArrow");
			GameObject gameObject2 = new GameObject("RightArrow");
			GameObject gameObject3 = new GameObject("RestartButton");
			GameObject gameObject4 = new GameObject("BackButton");
			gameObject.AddComponent<SpriteRenderer>();
			gameObject2.AddComponent<SpriteRenderer>();
			gameObject3.AddComponent<SpriteRenderer>();
			gameObject4.AddComponent<SpriteRenderer>();
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("left");
			gameObject2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("right");
			gameObject3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("restart");
			gameObject4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("back");
			gameObject.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
			gameObject2.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
			gameObject3.transform.position = new Vector3(screenWidth / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
			gameObject4.transform.position = new Vector3((0f - screenWidth) / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
			if (PlayerPrefs.GetInt("Setting3") == 1)
			{
				gameObject.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject2.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject3.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject4.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
			}
			else
			{
				gameObject.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject2.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject3.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject4.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
			}
		}
	}

	private void MoveBird()
	{
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
		if (PlayerPrefs.GetInt("Setting2") == 0)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				bird.transform.position += new Vector3(0f - num3, 0f, 0f);
				ClampPosition(num, bird);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				bird.transform.position += new Vector3(num3, 0f, 0f);
				ClampPosition(num, bird);
				bird.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
			}
			CheckIfGrounded();
			if (Input.GetKey(KeyCode.Space) && isGrounded)
			{
				rb.velocity = Vector2.up * jumpForce;
			}
			if (Input.GetKey(KeyCode.R))
			{
				bird.transform.position = new Vector3(0f, -4.3f, 0f);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
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
		GameObject gameObject = GameObject.Find("LeftArrow");
		GameObject gameObject2 = GameObject.Find("RightArrow");
		GameObject gameObject3 = GameObject.Find("RestartButton");
		GameObject gameObject4 = GameObject.Find("BackButton");
		if (Application.isMobilePlatform)
		{
			if (Input.touchCount <= 0)
			{
				return;
			}
			Touch touch = Input.GetTouch(0);
			Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
			point.z = 0f;
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
			{
				if (gameObject.GetComponent<SpriteRenderer>().bounds.Contains(point))
				{
					bird.transform.position += new Vector3(0f - num3, 0f, 0f);
					ClampPosition(num, bird);
					bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				}
				if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point))
				{
					bird.transform.position += new Vector3(num3, 0f, 0f);
					ClampPosition(num, bird);
					bird.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
				}
			}
			if (touch.phase != TouchPhase.Began)
			{
				return;
			}
			if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point))
			{
				bird.transform.position = new Vector3(0f, -4.3f, 0f);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
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
			if (gameObject4.GetComponent<SpriteRenderer>().bounds.Contains(point))
			{
				SceneManager.LoadScene("Menu");
			}
			return;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point2.z = 0f;
			if (gameObject.GetComponent<SpriteRenderer>().bounds.Contains(point2))
			{
				bird.transform.position += new Vector3(0f - num3, 0f, 0f);
				ClampPosition(num, bird);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			}
			else if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point2))
			{
				bird.transform.position += new Vector3(num3, 0f, 0f);
				ClampPosition(num, bird);
				bird.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 point3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			point3.z = 0f;
			if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point3))
			{
				bird.transform.position = new Vector3(0f, -4.3f, 0f);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				slownessLeft = 0f;
				UpdateScore(score);
				GameObject[] array10 = GameObject.FindGameObjectsWithTag("Berry");
				GameObject[] array11 = GameObject.FindGameObjectsWithTag("PoisonBerry");
				GameObject[] array12 = GameObject.FindGameObjectsWithTag("UltraBerry");
				GameObject[] array13 = GameObject.FindGameObjectsWithTag("SlowBerry");
				GameObject[] array5 = array10;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array11;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array12;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
				array5 = array13;
				for (int i = 0; i < array5.Length; i++)
				{
					UnityEngine.Object.Destroy(array5[i]);
				}
			}
			if (gameObject4.GetComponent<SpriteRenderer>().bounds.Contains(point3))
			{
				SceneManager.LoadScene("Menu");
			}
		}
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		Vector3 point4 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		point4.z = 0f;
		if (gameObject.GetComponent<SpriteRenderer>().bounds.Contains(point4))
		{
			bird.transform.position += new Vector3(0f - num3, 0f, 0f);
			ClampPosition(num, bird);
			bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
		}
		if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point4))
		{
			bird.transform.position += new Vector3(num3, 0f, 0f);
			ClampPosition(num, bird);
			bird.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
		}
		if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point4))
		{
			bird.transform.position = new Vector3(0f, -4.3f, 0f);
			bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
			score = 0;
			boostLeft = 0f;
			slownessLeft = 0f;
			UpdateScore(score);
			GameObject[] array14 = GameObject.FindGameObjectsWithTag("Berry");
			GameObject[] array15 = GameObject.FindGameObjectsWithTag("PoisonBerry");
			GameObject[] array16 = GameObject.FindGameObjectsWithTag("UltraBerry");
			GameObject[] array17 = GameObject.FindGameObjectsWithTag("SlowBerry");
			GameObject[] array5 = array14;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array15;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array16;
			for (int i = 0; i < array5.Length; i++)
			{
				UnityEngine.Object.Destroy(array5[i]);
			}
			array5 = array17;
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
		if (PlayerPrefs.GetInt("Setting2") == 0 && Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene("Menu");
		}
	}

	private void SpawnBerries()
	{
		if (Time.time >= nextSpawnTime)
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
		CheckIfGrounded();
		if (screenWidth != Camera.main.orthographicSize * 2f * Camera.main.aspect)
		{
			screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
			ClampPosition(screenWidth, GameObject.Find("Bird"));
			if (PlayerPrefs.GetInt("Setting2") == 1)
			{
				GameObject gameObject = GameObject.Find("LeftArrow");
				GameObject gameObject2 = GameObject.Find("RightArrow");
				GameObject gameObject3 = GameObject.Find("RestartButton");
				GameObject gameObject4 = GameObject.Find("BackButton");
				gameObject.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
				gameObject2.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
				gameObject3.transform.position = new Vector3(screenWidth / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
				gameObject4.transform.position = new Vector3((0f - screenWidth) / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
				if (PlayerPrefs.GetInt("Setting3") == 1)
				{
					gameObject.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject2.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject3.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject4.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				}
				else
				{
					gameObject.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject2.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject3.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject4.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				}
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("SlowBerry");
		GameObject[] array5 = array;
		foreach (GameObject gameObject5 in array5)
		{
			if (gameObject5.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject5);
			}
			else if (Vector3.Distance(bird.transform.position, gameObject5.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("eat"), Camera.main.transform.position);
				UnityEngine.Object.Destroy(gameObject5);
				score++;
				UpdateScore(score);
			}
		}
		array5 = array2;
		foreach (GameObject gameObject6 in array5)
		{
			if (gameObject6.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject6);
			}
			else if (Vector3.Distance(bird.transform.position, gameObject6.transform.position) < 1.5f)
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
				bird.transform.position = new Vector3(0f, -4.3f, 0f);
				bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
				score = 0;
				boostLeft = 0f;
				slownessLeft = 0f;
				UpdateScore(score);
			}
		}
		array5 = array3;
		foreach (GameObject gameObject7 in array5)
		{
			if (gameObject7.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject7);
			}
			else if (Vector3.Distance(bird.transform.position, gameObject7.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("powerup"), Camera.main.transform.position, 0.5f);
				UnityEngine.Object.Destroy(gameObject7);
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
		foreach (GameObject gameObject8 in array5)
		{
			if (gameObject8.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
			{
				UnityEngine.Object.Destroy(gameObject8);
			}
			else if (Vector3.Distance(bird.transform.position, gameObject8.transform.position) < 1.5f)
			{
				AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("slowness"), Camera.main.transform.position, 0.5f);
				UnityEngine.Object.Destroy(gameObject8);
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

	private void CheckIfGrounded()
	{
		isGrounded = bird.transform.position.y <= groundYPosition + 0.05f;
		rb.gravityScale = (isGrounded ? 0f : 1.5f);
		if (bird.transform.position.y < groundYPosition)
		{
			bird.transform.position = new Vector2(bird.transform.position.x, groundYPosition);
			rb.velocity = new Vector2(rb.velocity.x, 0f);
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
