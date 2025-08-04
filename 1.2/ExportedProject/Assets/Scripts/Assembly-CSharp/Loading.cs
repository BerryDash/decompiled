using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	private void Start()
	{
		if (!Application.isMobilePlatform)
		{
			QualitySettings.vSyncCount = PlayerPrefs.GetInt("Setting5", 0);
			if (PlayerPrefs.GetInt("Setting1", 1) == 1)
			{
				Screen.fullScreen = true;
			}
			else
			{
				Screen.fullScreen = false;
			}
		}
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			Screen.fullScreen = true;
		}
		SceneManager.LoadScene("Menu");
	}
}
