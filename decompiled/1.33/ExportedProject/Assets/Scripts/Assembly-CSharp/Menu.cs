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

	public GameObject valueModifier;

	private void Awake()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		if (PlayerPrefs.GetInt("secretSetting1", 0) == 1)
		{
			valueModifier.SetActive(value: true);
		}
		if (Application.isMobilePlatform)
		{
			exitBtn.gameObject.SetActive(value: false);
		}
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
		PlayerPrefs.SetInt("icon", Mathf.Clamp(PlayerPrefs.GetInt("icon", 0), 1, 8));
		PlayerPrefs.SetInt("overlay", Mathf.Clamp(PlayerPrefs.GetInt("overlay", 0), 0, 9));
		PlayerPrefs.Save();
	}
}
