using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
	public float spawnRate = 1f;

	private float nextSpawnTime;

	private int score;

	private int highscore;

	private float boostLeft;

	private float slownessLeft;

	private float screenWidth;

	private bool isGrounded;

	private float groundYPosition = -4.3f;

	public TMP_Text scoreText;

	public TMP_Text highScoreText;

	public TMP_Text boostText;

	public GameObject bird;

	public GameObject pausePanel;

	public Rigidbody2D rb;

	public AudioSource backgroundMusic;

	public TMP_Text fpsCounter;

	private float nextUpdate;

	private float fps;

	private void Awake()
	{
		highscore = PlayerPrefs.GetInt("HighScore", 0);
	}

	private void Start()
	{
		backgroundMusic.volume = PlayerPrefs.GetFloat("musicVolume", 1f);
		screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		GameObject.Find("HighScoreText").GetComponent<TMP_Text>().text = $"High Score: {highscore}";
		if (PlayerPrefs.GetInt("Setting2", 0) == 1 || Application.isMobilePlatform)
		{
			GameObject gameObject = new GameObject("LeftArrow");
			GameObject gameObject2 = new GameObject("RightArrow");
			GameObject gameObject3 = new GameObject("JumpArrow");
			GameObject gameObject4 = new GameObject("RestartButton");
			GameObject gameObject5 = new GameObject("BackButton");
			gameObject.AddComponent<SpriteRenderer>();
			gameObject2.AddComponent<SpriteRenderer>();
			gameObject3.AddComponent<SpriteRenderer>();
			gameObject4.AddComponent<SpriteRenderer>();
			gameObject5.AddComponent<SpriteRenderer>();
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arrow");
			gameObject2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arrow");
			gameObject3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arrow");
			gameObject4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Restart");
			gameObject5.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arrow");
			gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			gameObject2.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			gameObject5.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			gameObject.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
			gameObject2.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
			gameObject4.transform.position = new Vector3(screenWidth / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
			gameObject5.transform.position = new Vector3((0f - screenWidth) / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
			if (PlayerPrefs.GetInt("Setting3", 0) == 1)
			{
				gameObject.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject2.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject3.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject4.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject5.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
				gameObject3.transform.position = new Vector3(screenWidth / 2.5f, -1f, 0f);
			}
			else
			{
				gameObject.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject2.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject3.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject4.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject5.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
				gameObject3.transform.position = new Vector3(screenWidth / 2.5f, -2f, 0f);
			}
		}
	}

	private void MoveBird()
	{
		float num = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		float num2 = 0.18f * (num / 20.19257f);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		float num3 = num2;
		if (boostLeft > 0f)
		{
			num3 = num2 * 1.39f;
		}
		else if (slownessLeft > 0f)
		{
			num3 = num2 * 0.56f;
		}
		CheckIfGrounded();
		float axisRaw = Input.GetAxisRaw("Horizontal");
		if (!Application.isMobilePlatform)
		{
			if (axisRaw < 0f || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				flag2 = true;
			}
			if (axisRaw > 0f || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				flag = true;
			}
			if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || (Input.GetMouseButton(0) && PlayerPrefs.GetInt("Setting2", 0) == 0) || Input.GetKey(KeyCode.JoystickButton0))
			{
				flag3 = true;
			}
			if (Input.GetKey(KeyCode.R))
			{
				flag5 = true;
			}
		}
		if (PlayerPrefs.GetInt("Setting2", 0) != 0 || Application.isMobilePlatform)
		{
			GameObject gameObject = GameObject.Find("LeftArrow");
			GameObject gameObject2 = GameObject.Find("RightArrow");
			GameObject gameObject3 = GameObject.Find("JumpArrow");
			GameObject gameObject4 = GameObject.Find("RestartButton");
			GameObject gameObject5 = GameObject.Find("BackButton");
			if (Application.isMobilePlatform)
			{
				Touch[] touches = Input.touches;
				for (int i = 0; i < touches.Length; i++)
				{
					Touch touch = touches[i];
					Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
					point.z = 0f;
					if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
					{
						if (gameObject.GetComponent<SpriteRenderer>().bounds.Contains(point))
						{
							flag2 = true;
						}
						else if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point))
						{
							flag = true;
						}
						else if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point))
						{
							flag3 = true;
						}
					}
					if (touch.phase == TouchPhase.Began)
					{
						if (gameObject4.GetComponent<SpriteRenderer>().bounds.Contains(point))
						{
							flag5 = true;
						}
						else if (gameObject5.GetComponent<SpriteRenderer>().bounds.Contains(point))
						{
							flag4 = true;
						}
					}
				}
			}
			else
			{
				if (Input.GetMouseButton(0))
				{
					Vector3 point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					point2.z = 0f;
					if (gameObject.GetComponent<SpriteRenderer>().bounds.Contains(point2))
					{
						flag2 = true;
					}
					if (gameObject2.GetComponent<SpriteRenderer>().bounds.Contains(point2))
					{
						flag = true;
					}
					if (gameObject3.GetComponent<SpriteRenderer>().bounds.Contains(point2))
					{
						flag3 = true;
					}
				}
				if (Input.GetMouseButtonDown(0))
				{
					Vector3 point3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					point3.z = 0f;
					if (gameObject4.GetComponent<SpriteRenderer>().bounds.Contains(point3))
					{
						flag5 = true;
					}
					if (gameObject5.GetComponent<SpriteRenderer>().bounds.Contains(point3))
					{
						flag4 = true;
					}
				}
			}
		}
		if (flag2 && !flag)
		{
			bird.transform.position += new Vector3(0f - num3, 0f, 0f);
			ClampPosition(num, bird);
			bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
		}
		if (flag && !flag2)
		{
			bird.transform.position += new Vector3(num3, 0f, 0f);
			ClampPosition(num, bird);
			bird.transform.localScale = new Vector3(-1.35f, 1.35f, 1.35f);
		}
		if (flag3 && isGrounded)
		{
			AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("jump"), Camera.main.transform.position, 0.75f * PlayerPrefs.GetFloat("sfxVolume", 1f));
			if (boostLeft > 0f)
			{
				rb.velocity = Vector2.up * 12f;
			}
			else if (slownessLeft > 0f)
			{
				rb.velocity = Vector2.up * 6f;
			}
			else
			{
				rb.velocity = Vector2.up * 9f;
			}
		}
		if (flag4)
		{
			TogglePause();
		}
		if (flag5)
		{
			Respawn();
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
		SpawnBerries();
		if (!pausePanel.active)
		{
			MoveBird();
			if (boostLeft > 0f)
			{
				boostLeft -= Time.deltaTime;
				boostText.GetComponent<TMP_Text>().text = "Boost expires in " + $"{boostLeft:0.0}" + "s";
			}
			else if (slownessLeft > 0f)
			{
				slownessLeft -= Time.deltaTime;
				boostText.GetComponent<TMP_Text>().text = "Slowness expires in " + $"{slownessLeft:0.0}" + "s";
			}
			else
			{
				boostText.GetComponent<TMP_Text>().text = "";
			}
		}
	}

	private void SpawnBerries()
	{
		if (!(Time.time >= nextSpawnTime))
		{
			return;
		}
		nextSpawnTime = Time.time + 1f / spawnRate;
		float value = Random.value;
		if (!pausePanel.active)
		{
			GameObject gameObject;
			SpriteRenderer spriteRenderer;
			if (value <= 0.6f)
			{
				gameObject = new GameObject("Berry");
				spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = Resources.Load<Sprite>("Berry");
				gameObject.tag = "Berry";
			}
			else if (value <= 0.8f)
			{
				gameObject = new GameObject("PoisonBerry");
				spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = Resources.Load<Sprite>("PoisonBerry");
				gameObject.tag = "PoisonBerry";
			}
			else if (value <= 0.9f)
			{
				gameObject = new GameObject("SlowBerry");
				spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = Resources.Load<Sprite>("SlowBerry");
				gameObject.tag = "SlowBerry";
			}
			else
			{
				gameObject = new GameObject("UltraBerry");
				spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = Resources.Load<Sprite>("UltraBerry");
				gameObject.tag = "UltraBerry";
			}
			spriteRenderer.sortingOrder = -5;
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
		if (PlayerPrefs.GetInt("Setting4", 0) == 1 && Time.time > nextUpdate)
		{
			fps = 1f / Time.deltaTime;
			fpsCounter.text = "FPS: " + Mathf.Round(fps);
			nextUpdate = Time.time + 0.25f;
		}
		if (screenWidth != Camera.main.orthographicSize * 2f * Camera.main.aspect)
		{
			screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
			ClampPosition(screenWidth, bird);
			if (PlayerPrefs.GetInt("Setting2", 0) == 1)
			{
				GameObject gameObject = GameObject.Find("LeftArrow");
				GameObject gameObject2 = GameObject.Find("RightArrow");
				GameObject gameObject3 = GameObject.Find("JumpArrow");
				GameObject gameObject4 = GameObject.Find("RestartButton");
				GameObject gameObject5 = GameObject.Find("BackButton");
				gameObject.transform.position = new Vector3((0f - screenWidth) / 2.5f, -4f, 0f);
				gameObject2.transform.position = new Vector3(screenWidth / 2.5f, -4f, 0f);
				gameObject4.transform.position = new Vector3(screenWidth / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
				gameObject5.transform.position = new Vector3((0f - screenWidth) / 2.3f, Camera.main.orthographicSize - 1.2f, 0f);
				if (PlayerPrefs.GetInt("Setting3", 0) == 1)
				{
					gameObject.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject2.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject3.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject4.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject5.transform.localScale = new Vector3(screenWidth / 14f, screenWidth / 14f, 1f);
					gameObject3.transform.position = new Vector3(screenWidth / 2.5f, -1f, 0f);
				}
				else
				{
					gameObject.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject2.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject3.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject4.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject5.transform.localScale = new Vector3(screenWidth / 20f, screenWidth / 20f, 1f);
					gameObject3.transform.position = new Vector3(screenWidth / 2.5f, -2f, 0f);
				}
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Berry");
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("PoisonBerry");
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("UltraBerry");
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("SlowBerry");
		if (!pausePanel.active)
		{
			CheckIfGrounded();
			GameObject[] array5 = array;
			foreach (GameObject gameObject6 in array5)
			{
				if (gameObject6.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
				{
					Object.Destroy(gameObject6);
				}
				else if (Vector3.Distance(bird.transform.position, gameObject6.transform.position) < 1.5f)
				{
					AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("eat"), Camera.main.transform.position, PlayerPrefs.GetFloat("sfxVolume", 1f));
					Object.Destroy(gameObject6);
					score++;
					UpdateScore(score);
				}
				gameObject6.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -3f);
			}
			array5 = array2;
			foreach (GameObject gameObject7 in array5)
			{
				if (gameObject7.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
				{
					Object.Destroy(gameObject7);
				}
				else if (Vector3.Distance(bird.transform.position, gameObject7.transform.position) < 1.5f)
				{
					AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("death"), Camera.main.transform.position, PlayerPrefs.GetFloat("sfxVolume", 1f));
					Respawn();
				}
				gameObject7.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -3f);
			}
			array5 = array3;
			foreach (GameObject gameObject8 in array5)
			{
				if (gameObject8.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
				{
					Object.Destroy(gameObject8);
				}
				else if (Vector3.Distance(bird.transform.position, gameObject8.transform.position) < 1.5f)
				{
					AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("powerup"), Camera.main.transform.position, 0.5f * PlayerPrefs.GetFloat("sfxVolume", 1f));
					Object.Destroy(gameObject8);
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
				gameObject8.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -3f);
			}
			array5 = array4;
			foreach (GameObject gameObject9 in array5)
			{
				if (gameObject9.transform.position.y < 0f - Camera.main.orthographicSize - 1f)
				{
					Object.Destroy(gameObject9);
				}
				else if (Vector3.Distance(bird.transform.position, gameObject9.transform.position) < 1.5f)
				{
					AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("slowness"), Camera.main.transform.position, 0.5f * PlayerPrefs.GetFloat("sfxVolume", 1f));
					Object.Destroy(gameObject9);
					boostLeft = 0f;
					slownessLeft = 10f;
					if (score > 0)
					{
						score--;
						UpdateScore(score);
					}
				}
				gameObject9.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -3f);
			}
		}
		else
		{
			rb.gravityScale = 0f;
			rb.velocity = Vector2.zero;
			GameObject[] array5 = array;
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			array5 = array2;
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			array5 = array3;
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			array5 = array4;
			for (int i = 0; i < array5.Length; i++)
			{
				array5[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Joystick2Button7))
		{
			TogglePause();
		}
	}

	private void Respawn()
	{
		bird.transform.position = new Vector3(0f, -4.3f, 0f);
		bird.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
		rb.gravityScale = 0f;
		rb.velocity = Vector2.zero;
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
			Object.Destroy(array5[i]);
		}
		array5 = array2;
		for (int i = 0; i < array5.Length; i++)
		{
			Object.Destroy(array5[i]);
		}
		array5 = array3;
		for (int i = 0; i < array5.Length; i++)
		{
			Object.Destroy(array5[i]);
		}
		array5 = array4;
		for (int i = 0; i < array5.Length; i++)
		{
			Object.Destroy(array5[i]);
		}
	}

	private void UpdateScore(int score)
	{
		if (score > highscore)
		{
			highscore = score;
		}
		PlayerPrefs.SetInt("HighScore", highscore);
		PlayerPrefs.Save();
		scoreText.GetComponent<TMP_Text>().text = "Score: " + score;
		highScoreText.GetComponent<TMP_Text>().text = "High Score: " + highscore;
	}

	private void CheckIfGrounded()
	{
		GameObject gameObject = GameObject.Find("JumpArrow");
		isGrounded = bird.transform.position.y <= groundYPosition + 0.05f;
		rb.gravityScale = (isGrounded ? 0f : 1.5f);
		if (bird.transform.position.y < groundYPosition)
		{
			bird.transform.position = new Vector2(bird.transform.position.x, groundYPosition);
			rb.velocity = new Vector2(rb.velocity.x, 0f);
		}
		if (gameObject != null)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.material.color = (isGrounded ? Color.white : Color.red);
			}
		}
	}

	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("HighScore", highscore);
		PlayerPrefs.Save();
	}

	private void TogglePause()
	{
		if (pausePanel.active)
		{
			backgroundMusic.Play();
			pausePanel.SetActive(value: false);
		}
		else
		{
			backgroundMusic.Pause();
			pausePanel.SetActive(value: true);
		}
	}
}
