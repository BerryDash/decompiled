using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Account : MonoBehaviour
{
	public GameObject registerPanel;

	public Button registerPanelLoginBtn;

	public GameObject loginPanel;

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

	public GameObject changeUsernamePanel;

	public TMP_Text changeUsernamePanelStatusText;

	public Button submitButton;

	public Button changeUsernameButton;

	public Button changePasswordButton;

	public GameObject changePasswordPanel;

	public TMP_Text changePasswordStatusText;

	public TMP_Text changeUsernameStatusText;

	public TMP_InputField changeUsernameCurrentForm;

	public TMP_InputField changeUsernameNewForm;

	public TMP_InputField changeUsernamePasswordForm;

	public TMP_InputField changePasswordCurrentUsernameForm;

	public TMP_InputField changePasswordCurrentForm;

	public TMP_InputField changePasswordNewForm;

	private void Awake()
	{
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
		submitButton.onClick.AddListener(ButtonSubmit);
		loginPanelRegisterBtn.onClick.AddListener(delegate
		{
			SwitchScene(1);
		});
		accountPanelSyncButton.onClick.AddListener(StartSyncAccount);
		accountPanelLogoutButton.onClick.AddListener(LogoutAccount);
		changeUsernameButton.onClick.AddListener(delegate
		{
			SwitchScene(3);
		});
		changePasswordButton.onClick.AddListener(delegate
		{
			SwitchScene(4);
		});
	}

	private void BackToMenu()
	{
		if (GetCurrentScene() == 3 || GetCurrentScene() == 4)
		{
			SwitchScene(0);
		}
		else
		{
			SceneManager.LoadScene("Menu");
		}
	}

	private int GetCurrentScene()
	{
		if (accountPanel.activeSelf)
		{
			return 0;
		}
		if (registerPanel.activeSelf)
		{
			return 1;
		}
		if (loginPanel.activeSelf)
		{
			return 2;
		}
		if (changeUsernamePanel.activeSelf)
		{
			return 3;
		}
		if (changePasswordPanel.activeSelf)
		{
			return 4;
		}
		return -1;
	}

	private void ButtonSubmit()
	{
		if (GetCurrentScene() == 1)
		{
			StartCoroutine(SubmitRegister());
		}
		else if (GetCurrentScene() == 2)
		{
			StartCoroutine(SubmitLogin());
		}
		else if (GetCurrentScene() == 3)
		{
			StartCoroutine(ChangeUsername());
		}
		else if (GetCurrentScene() == 4)
		{
			StartCoroutine(ChangePassword());
		}
	}

	private void SwitchScene(int scene)
	{
		ClearForms();
		backButton.transform.localPosition = new Vector3(0f, -165f, 0f);
		backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 30f);
		submitButton.transform.localPosition = new Vector3(110f, -165f, 0f);
		submitButton.gameObject.SetActive(value: true);
		accountPanel.SetActive(value: false);
		registerPanel.SetActive(value: false);
		loginPanel.SetActive(value: false);
		changeUsernamePanel.SetActive(value: false);
		changePasswordPanel.SetActive(value: false);
		registerPanelStatusText.text = "";
		loginPanelStatusText.text = "";
		accountPanelStatusText.text = "";
		changeUsernamePanelStatusText.text = "";
		changePasswordStatusText.text = "";
		changeUsernameStatusText.text = "";
		registerPanelStatusText.color = Color.white;
		loginPanelStatusText.color = Color.white;
		accountPanelStatusText.color = Color.white;
		changeUsernamePanelStatusText.color = Color.white;
		changePasswordStatusText.color = Color.white;
		changeUsernameStatusText.color = Color.white;
		accountPanelSyncButton.interactable = true;
		switch (scene)
		{
		case 0:
			accountPanel.SetActive(value: true);
			submitButton.gameObject.SetActive(value: false);
			backButton.transform.localPosition = new Vector3(-60f, -165f, 0f);
			backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(110f, 30f);
			break;
		case 1:
			registerPanel.SetActive(value: true);
			break;
		case 2:
			loginPanel.SetActive(value: true);
			break;
		case 3:
			changeUsernamePanel.SetActive(value: true);
			backButton.transform.localPosition = new Vector3(-60f, -165f, 0f);
			backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(110f, 30f);
			submitButton.transform.localPosition = new Vector3(60f, -165f, 0f);
			break;
		case 4:
			changePasswordPanel.SetActive(value: true);
			backButton.transform.localPosition = new Vector3(-60f, -165f, 0f);
			backButton.GetComponent<RectTransform>().sizeDelta = new Vector2(110f, 30f);
			submitButton.transform.localPosition = new Vector3(60f, -165f, 0f);
			break;
		}
	}

	private IEnumerator SubmitRegister()
	{
		if (!registerPanelEmailForm.text.Trim().Equals(registerPanelEmailConfirmForm.text.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			UpdateStatusText(registerPanelStatusText, "Email doesn't match", Color.red);
			yield break;
		}
		if (!registerPanelPasswordForm.text.Trim().Equals(registerPanelPasswordConfirmForm.text.Trim(), StringComparison.OrdinalIgnoreCase))
		{
			UpdateStatusText(registerPanelStatusText, "Password doesn't match", Color.red);
			yield break;
		}
		if (!Regex.IsMatch(registerPanelUsernameForm.text, "^[a-zA-Z0-9]{3,16}$"))
		{
			UpdateStatusText(registerPanelStatusText, "Username must be 3-16 characters, letters and numbers only", Color.red);
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("username", registerPanelUsernameForm.text);
		wWWForm.AddField("email", registerPanelEmailForm.text);
		wWWForm.AddField("password", registerPanelPasswordForm.text);
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/registerAccount.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("HTTP Error: " + request.error);
			UpdateStatusText(registerPanelStatusText, "Failed to make HTTP request", Color.red);
			yield break;
		}
		string text = request.downloadHandler.text;
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

	private IEnumerator SubmitLogin()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("username", loginPanelUsernameForm.text);
		wWWForm.AddField("password", loginPanelPasswordForm.text);
		wWWForm.AddField("currentHighScore", PlayerPrefs.GetInt("HighScore", 0).ToString());
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/loginAccount.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("HTTP Error: " + request.error);
			UpdateStatusText(loginPanelStatusText, "Failed to make HTTP request", Color.red);
			yield break;
		}
		string text = request.downloadHandler.text;
		Debug.Log(text);
		if (!(text == "-1"))
		{
			if (text == "-2")
			{
				UpdateStatusText(loginPanelStatusText, "Incorrect username or password", Color.red);
			}
			else if (Regex.IsMatch(text, "^[a-zA-Z0-9]{512}:\\d+:\\d+:\\d+:\\d+$"))
			{
				string[] array = text.Split(':');
				string value = array[0];
				int value2 = int.Parse(array[1]);
				int value3 = int.Parse(array[2]);
				int value4 = int.Parse(array[3]);
				int value5 = int.Parse(array[4]);
				PlayerPrefs.SetString("gameSession", value);
				PlayerPrefs.SetInt("userID", value2);
				PlayerPrefs.SetInt("HighScore", value3);
				PlayerPrefs.SetInt("icon", value4);
				PlayerPrefs.SetInt("overlay", value5);
				SwitchScene(0);
				UpdateStatusText(loginPanelStatusText, "", Color.red);
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

	private IEnumerator ChangeUsername()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("username", changeUsernameCurrentForm.text);
		wWWForm.AddField("currentPassword", changeUsernamePasswordForm.text);
		wWWForm.AddField("newUsername", changeUsernameNewForm.text);
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/changeAccountUsername.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("HTTP Error: " + request.error);
			UpdateStatusText(changeUsernameStatusText, "Failed to make HTTP request", Color.red);
			yield break;
		}
		string text = request.downloadHandler.text;
		Debug.Log(text);
		switch (text)
		{
		case "1":
			SwitchScene(0);
			UpdateStatusText(accountPanelStatusText, "Username changed successfully", Color.green);
			break;
		case "-1":
			UpdateStatusText(changeUsernameStatusText, "Internal login server error", Color.red);
			break;
		case "-2":
			UpdateStatusText(changeUsernameStatusText, "New Username, Password, or old username is empty", Color.red);
			break;
		case "-3":
			UpdateStatusText(changeUsernameStatusText, "New Username contains invalid characters", Color.red);
			break;
		case "-4":
			UpdateStatusText(changeUsernameStatusText, "New Username is too short or too long", Color.red);
			break;
		case "-5":
			UpdateStatusText(changeUsernameStatusText, "New Username does not match the required format", Color.red);
			break;
		case "-6":
			UpdateStatusText(changeUsernameStatusText, "Incorrect current password", Color.red);
			break;
		case "-7":
			UpdateStatusText(changeUsernameStatusText, "Current username is incorrect", Color.red);
			break;
		case "-8":
			UpdateStatusText(changeUsernameStatusText, "New username already exists", Color.red);
			break;
		default:
			UpdateStatusText(changeUsernameStatusText, "Unknown server response", Color.red);
			break;
		}
	}

	private IEnumerator ChangePassword()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("username", changePasswordCurrentUsernameForm.text);
		wWWForm.AddField("currentPassword", changePasswordCurrentForm.text);
		wWWForm.AddField("newPassword", changePasswordNewForm.text);
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/changeAccountPassword.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("HTTP Error: " + request.error);
			UpdateStatusText(changePasswordStatusText, "Failed to make HTTP request", Color.red);
			yield break;
		}
		string text = request.downloadHandler.text;
		Debug.Log(text);
		switch (text)
		{
		case "-1":
			UpdateStatusText(changePasswordStatusText, "Internal login server error", Color.red);
			yield break;
		case "-2":
			UpdateStatusText(changePasswordStatusText, "New Password, Password, or username is empty", Color.red);
			yield break;
		case "-3":
			UpdateStatusText(changePasswordStatusText, "New Password is too short or too long", Color.red);
			yield break;
		case "-4":
			UpdateStatusText(changePasswordStatusText, "Username must be 3-16 characters, letters and numbers only", Color.red);
			yield break;
		case "-5":
			UpdateStatusText(changePasswordStatusText, "Incorrect current password", Color.red);
			yield break;
		case "-6":
			UpdateStatusText(changePasswordStatusText, "Current username is incorrect", Color.red);
			yield break;
		case "-7":
			UpdateStatusText(changePasswordStatusText, "New password cannot be the same as your old password", Color.red);
			yield break;
		}
		if (Regex.IsMatch(text, "^[a-zA-Z0-9]{512}$"))
		{
			PlayerPrefs.SetString("gameSession", text);
			SwitchScene(0);
			UpdateStatusText(accountPanelStatusText, "Password changed successfully", Color.green);
		}
		else
		{
			UpdateStatusText(accountPanelStatusText, "Unknown server response", Color.red);
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
		changeUsernameNewForm.text = "";
		changeUsernamePasswordForm.text = "";
		changePasswordCurrentForm.text = "";
		changePasswordNewForm.text = "";
		changePasswordCurrentUsernameForm.text = "";
		changeUsernameCurrentForm.text = "";
	}

	private IEnumerator SyncAccount()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userID", PlayerPrefs.GetInt("userID", 0).ToString());
		wWWForm.AddField("gameSession", PlayerPrefs.GetString("gameSession", ""));
		wWWForm.AddField("highScore", PlayerPrefs.GetInt("HighScore", 0).ToString());
		wWWForm.AddField("icon", PlayerPrefs.GetInt("icon", 1).ToString());
		wWWForm.AddField("overlay", PlayerPrefs.GetInt("overlay", 0).ToString());
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/syncAccount.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("HTTP Error: " + request.error);
			UpdateStatusText(accountPanelStatusText, "Failed to make HTTP request", Color.red);
			yield break;
		}
		string text = request.downloadHandler.text;
		Debug.Log(text);
		switch (text)
		{
		case "1":
			accountPanelSyncButton.interactable = false;
			UpdateStatusText(accountPanelStatusText, "Synced account", Color.green);
			break;
		case "-1":
			UpdateStatusText(accountPanelStatusText, "Internal login server error", Color.red);
			break;
		case "-2":
			LogoutAccount();
			break;
		default:
			UpdateStatusText(accountPanelStatusText, "Unknown server response \"" + text + "\"", Color.red);
			break;
		}
	}

	private void StartSyncAccount()
	{
		StartCoroutine(SyncAccount());
	}

	private void LogoutAccount()
	{
		PlayerPrefs.DeleteKey("userID");
		PlayerPrefs.DeleteKey("gameSession");
		PlayerPrefs.SetInt("HighScore", 0);
		PlayerPrefs.SetInt("icon", 1);
		PlayerPrefs.SetInt("overlay", 0);
		SwitchScene(1);
	}

	private void UpdateStatusText(TMP_Text text, string message, Color color)
	{
		text.text = message;
		text.color = color;
	}
}
