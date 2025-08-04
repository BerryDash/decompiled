using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
	private readonly string GameSession;

	public GameObject registerPanel;

	public Button registerPanelSubmitBtn;

	public Button registerPanelLoginBtn;

	public GameObject loginPanel;

	public Button loginPanelSubmitBtn;

	public Button loginPanelRegisterBtn;

	public GameObject accountPanel;

	public Button backButton;

	public Button accountPanelSyncButton;

	public Button accountPanelLogoutButton;

	public TMP_InputField registerPanelUsernameForm;

	public TMP_InputField registerPanelEmailForm;

	public TMP_InputField registerPanelEmailConfirmForm;

	public TMP_InputField registerPanelPasswordForm;

	public TMP_InputField registerPanelPasswordConfirmForm;

	public TMP_InputField loginPanelUsernameForm;

	public TMP_InputField loginPanelPasswordForm;

	public TMP_Text registerPanelStatusText;

	public TMP_Text loginPanelStatusText;

	public TMP_Text accountPanelStatusText;

	private static readonly HttpClient client = new HttpClient();

	private void Awake()
	{
		client.DefaultRequestHeaders.Add("User-Agent", "BerryDashClient");
		if (PlayerPrefs.HasKey("gameSession") && PlayerPrefs.HasKey("userID"))
		{
			SwitchScene(0);
		}
		else
		{
			SwitchScene(1);
		}
		backButton.onClick.AddListener(BackToMenu);
		registerPanelLoginBtn.onClick.AddListener(delegate
		{
			SwitchScene(2);
		});
		registerPanelSubmitBtn.onClick.AddListener(SubmitRegister);
		loginPanelRegisterBtn.onClick.AddListener(delegate
		{
			SwitchScene(1);
		});
		loginPanelSubmitBtn.onClick.AddListener(SubmitLogin);
		accountPanelSyncButton.onClick.AddListener(SyncStats);
		accountPanelLogoutButton.onClick.AddListener(LogoutAccount);
	}

	private void BackToMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	private void SwitchScene(int scene)
	{
		ClearForms();
		registerPanelStatusText.text = "";
		loginPanelStatusText.text = "";
		accountPanelStatusText.text = "";
		switch (scene)
		{
		case 0:
			accountPanel.SetActive(value: true);
			registerPanel.SetActive(value: false);
			loginPanel.SetActive(value: false);
			break;
		case 1:
			accountPanel.SetActive(value: false);
			registerPanel.SetActive(value: true);
			loginPanel.SetActive(value: false);
			break;
		case 2:
			accountPanel.SetActive(value: false);
			registerPanel.SetActive(value: false);
			loginPanel.SetActive(value: true);
			break;
		}
	}

	private async void SubmitRegister()
	{
		if (!registerPanelEmailForm.text.Trim().Equals(registerPanelEmailConfirmForm.text.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			UpdateStatusText(registerPanelStatusText, "Email doesn't match", Color.red);
			return;
		}
		if (!registerPanelPasswordForm.text.Trim().Equals(registerPanelPasswordConfirmForm.text.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			UpdateStatusText(registerPanelStatusText, "Password doesn't match", Color.red);
			return;
		}
		if (!Regex.IsMatch(registerPanelUsernameForm.text, "^[a-zA-Z0-9]{3,16}$"))
		{
			UpdateStatusText(registerPanelStatusText, "Username must be 3-16 characters, letters and numbers only", Color.red);
			return;
		}
		try
		{
			HttpResponseMessage obj = await client.PostAsync("https://berrydash.xytriza.com/api/registerAccount.php", new FormUrlEncodedContent(new KeyValuePair<string, string>[3]
			{
				new KeyValuePair<string, string>("username", registerPanelUsernameForm.text),
				new KeyValuePair<string, string>("email", registerPanelEmailForm.text),
				new KeyValuePair<string, string>("password", registerPanelPasswordForm.text)
			}));
			obj.EnsureSuccessStatusCode();
			string text = await obj.Content.ReadAsStringAsync();
			Debug.Log(text);
			switch (text)
			{
			case "1":
				SwitchScene(2);
				break;
			case "-1":
				UpdateStatusText(registerPanelStatusText, "Internal login server error", Color.red);
				break;
			case "-2":
				UpdateStatusText(registerPanelStatusText, "Incomplete form data", Color.red);
				break;
			case "-3":
				UpdateStatusText(registerPanelStatusText, "Username not valid", Color.red);
				break;
			case "-4":
				UpdateStatusText(registerPanelStatusText, "Email not valid", Color.red);
				break;
			case "-5":
				UpdateStatusText(registerPanelStatusText, "Password must have 8 characters, one number and one letter", Color.red);
				break;
			case "-6":
				UpdateStatusText(registerPanelStatusText, "Username too long or short", Color.red);
				break;
			case "-7":
				UpdateStatusText(registerPanelStatusText, "Username must be 3-16 characters, letters and numbers only", Color.red);
				break;
			case "-8":
				UpdateStatusText(registerPanelStatusText, "Username or email already exists", Color.red);
				break;
			default:
				UpdateStatusText(registerPanelStatusText, "Unknown server response \"" + text + "\"", Color.red);
				break;
			}
		}
		catch (HttpRequestException arg)
		{
			Debug.LogError($"HTTP Error: {arg}");
			UpdateStatusText(registerPanelStatusText, "Failed to make HTTP request", Color.red);
		}
	}

	private async void SubmitLogin()
	{
		_ = 1;
		try
		{
			HttpResponseMessage obj = await client.PostAsync("https://berrydash.xytriza.com/api/loginAccount.php", new FormUrlEncodedContent(new KeyValuePair<string, string>[2]
			{
				new KeyValuePair<string, string>("username", loginPanelUsernameForm.text),
				new KeyValuePair<string, string>("password", loginPanelPasswordForm.text)
			}));
			obj.EnsureSuccessStatusCode();
			string text = await obj.Content.ReadAsStringAsync();
			if (!(text == "-1"))
			{
				if (text == "-2")
				{
					UpdateStatusText(loginPanelStatusText, "Incorrect username or password", Color.red);
				}
				else if (Regex.IsMatch(text, "^[a-zA-Z0-9]+:\\d+:\\d+$"))
				{
					string[] array = text.Split(':');
					string value = array[0];
					int value2 = int.Parse(array[1]);
					int value3 = int.Parse(array[2]);
					PlayerPrefs.SetString("gameSession", value);
					PlayerPrefs.SetInt("userID", value2);
					PlayerPrefs.SetInt("HighScore", value3);
					SwitchScene(0);
				}
				else
				{
					UpdateStatusText(loginPanelStatusText, "Unknown server response", Color.red);
				}
			}
			else
			{
				UpdateStatusText(loginPanelStatusText, "Internal login server error", Color.red);
			}
		}
		catch (HttpRequestException arg)
		{
			Debug.LogError($"HTTP Error: {arg}");
			UpdateStatusText(loginPanelStatusText, "Failed to make HTTP request", Color.red);
		}
	}

	private void ClearForms()
	{
		registerPanelUsernameForm.text = "";
		registerPanelEmailForm.text = "";
		registerPanelEmailConfirmForm.text = "";
		registerPanelPasswordForm.text = "";
		registerPanelPasswordConfirmForm.text = "";
		loginPanelUsernameForm.text = "";
		loginPanelPasswordForm.text = "";
	}

	private async void SyncStats()
	{
		_ = 1;
		try
		{
			HttpResponseMessage obj = await client.PostAsync("https://berrydash.xytriza.com/api/syncAccount.php", new FormUrlEncodedContent(new KeyValuePair<string, string>[3]
			{
				new KeyValuePair<string, string>("userID", PlayerPrefs.GetInt("userID", 0).ToString()),
				new KeyValuePair<string, string>("gameSession", PlayerPrefs.GetString("gameSession", "")),
				new KeyValuePair<string, string>("highScore", PlayerPrefs.GetInt("HighScore", 0).ToString())
			}));
			obj.EnsureSuccessStatusCode();
			string text = await obj.Content.ReadAsStringAsync();
			Debug.Log(text);
			switch (text)
			{
			case "1":
				UpdateStatusText(accountPanelStatusText, "Updated leaderboard stats", Color.green);
				break;
			case "-1":
				UpdateStatusText(accountPanelStatusText, "Internal login server error", Color.red);
				break;
			case "-2":
				UpdateStatusText(accountPanelStatusText, "Stats already synced", Color.red);
				break;
			case "-3":
				UpdateStatusText(accountPanelStatusText, "Invalid game session (re-login)", Color.red);
				break;
			default:
				UpdateStatusText(accountPanelStatusText, "Unknown server response \"" + text + "\"", Color.red);
				break;
			}
		}
		catch (HttpRequestException arg)
		{
			Debug.LogError($"HTTP Error: {arg}");
			UpdateStatusText(accountPanelStatusText, "Failed to make HTTP request", Color.red);
		}
	}

	private void LogoutAccount()
	{
		PlayerPrefs.DeleteKey("userID");
		PlayerPrefs.DeleteKey("gameSession");
		PlayerPrefs.DeleteKey("HighScore");
		SwitchScene(1);
	}

	private void UpdateStatusText(TMP_Text text, string message, Color color)
	{
		text.text = message;
		text.color = color;
	}
}
