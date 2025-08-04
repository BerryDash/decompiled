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
		PlayerPrefs.SetFloat("latestVersion", clientVersion);
		if (!PlayerPrefs.HasKey("iconSelected"))
		{
			PlayerPrefs.SetInt("iconSelected", 1);
		}
		SceneManager.LoadScene("Menu");
	}
}
