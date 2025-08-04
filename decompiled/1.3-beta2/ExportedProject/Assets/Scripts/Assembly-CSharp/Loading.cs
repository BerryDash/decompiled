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
		PlayerPrefs.SetInt("icon", Mathf.Clamp(PlayerPrefs.GetInt("icon", 0), 1, 4));
		PlayerPrefs.SetInt("overlay", Mathf.Clamp(PlayerPrefs.GetInt("overlay", 0), 0, 9));
		PlayerPrefs.Save();
		SceneManager.LoadScene("Menu");
	}
}
