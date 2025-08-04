using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public RuntimePlatform[] desktopPlatforms = new RuntimePlatform[3]
	{
		RuntimePlatform.WindowsPlayer,
		RuntimePlatform.LinuxPlayer,
		RuntimePlatform.OSXPlayer
	};

	public GameObject exitObj;

	public Button playBtn;

	public Button settingsBtn;

	public Button accountBtn;

	public Button leaderboardBtn;

	private void Awake()
	{
		Button component = exitObj.GetComponent<Button>();
		if (desktopPlatforms.Contains(Application.platform))
		{
			component.onClick.AddListener(ExitClick);
		}
		else
		{
			Object.Destroy(exitObj);
		}
		playBtn.onClick.AddListener(PlayClick);
		settingsBtn.onClick.AddListener(SettingsClick);
		accountBtn.onClick.AddListener(AccountClick);
		leaderboardBtn.onClick.AddListener(LeaderboardClick);
	}

	private void PlayClick()
	{
		SceneManager.LoadScene("Game");
	}

	private void SettingsClick()
	{
		SceneManager.LoadScene("Settings");
	}

	private void AccountClick()
	{
		SceneManager.LoadScene("Account");
	}

	private void LeaderboardClick()
	{
		SceneManager.LoadScene("Leaderboard");
	}

	private void ExitClick()
	{
		Application.Quit();
	}
}
