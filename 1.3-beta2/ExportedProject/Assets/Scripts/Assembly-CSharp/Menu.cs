using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public Button exitBtn;

	public Button playBtn;

	public Button settingsBtn;

	public Button accountBtn;

	public Button leaderboardBtn;

	public Button iconkitButton;

	private void Awake()
	{
		exitBtn.onClick.AddListener(delegate
		{
			Application.Quit();
		});
		playBtn.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Game");
		});
		settingsBtn.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Settings");
		});
		accountBtn.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Account");
		});
		leaderboardBtn.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Leaderboard");
		});
		iconkitButton.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Iconkit");
		});
		PlayerPrefs.SetInt("icon", Mathf.Clamp(PlayerPrefs.GetInt("icon", 0), 1, 4));
		PlayerPrefs.SetInt("overlay", Mathf.Clamp(PlayerPrefs.GetInt("overlay", 0), 0, 9));
		PlayerPrefs.Save();
	}
}
