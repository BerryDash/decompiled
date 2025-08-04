using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	private void Awake()
	{
		QualitySettings.vSyncCount = PlayerPrefs.GetInt("Setting5", 0);
		Screen.fullScreen = PlayerPrefs.GetInt("Setting1", 1) == 1;
		PlayerPrefs.SetString("latestVersion", "1.21");
		SceneManager.LoadScene("Menu");
	}
}
