using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	public string clientVersion;

	private void Awake()
	{
		QualitySettings.vSyncCount = PlayerPrefs.GetInt("Setting5", 1);
		Screen.fullScreen = PlayerPrefs.GetInt("Setting1", 1) == 1;
		PlayerPrefs.SetString("latestVersion", clientVersion);
		if (!PlayerPrefs.HasKey("iconSelected"))
		{
			PlayerPrefs.SetInt("iconSelected", 1);
		}
		SceneManager.LoadScene("Menu");
	}
}
