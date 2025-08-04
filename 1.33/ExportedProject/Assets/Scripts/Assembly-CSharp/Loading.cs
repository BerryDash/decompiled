using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	public float clientVersion;

	public TMP_Text statusText;

	public Button updateButton;

	private void Awake()
	{
		QualitySettings.vSyncCount = PlayerPrefs.GetInt("Setting5", 1);
		Screen.fullScreen = PlayerPrefs.GetInt("Setting1", 1) == 1;
		if (!Application.isMobilePlatform)
		{
			SetIfNone("Setting1", 1);
			SetIfNone("Setting2", 0);
			SetIfNone("Setting3", 0);
			SetIfNone("Setting4", 0);
			SetIfNone("Setting5", 1);
		}
		else
		{
			SetIfNone("Setting1", 1, overrideValue: true);
			SetIfNone("Setting2", 1, overrideValue: true);
			SetIfNone("Setting3", 0);
			SetIfNone("Setting4", 0);
			SetIfNone("Setting5", 1, overrideValue: true);
		}
		PlayerPrefs.SetFloat("latestVersion", clientVersion);
		SceneManager.LoadScene("Menu");
	}

	private void SetIfNone(string key, int value, bool overrideValue = false)
	{
		if (!PlayerPrefs.HasKey(key) || overrideValue)
		{
			PlayerPrefs.SetInt(key, value);
		}
	}
}
