using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	private void Awake()
	{
		if (!PlayerPrefs.HasKey("Setting1"))
		{
			if (!Application.isMobilePlatform)
			{
				PlayerPrefs.SetInt("Setting1", 1);
			}
			else
			{
				PlayerPrefs.SetInt("Setting1", 1);
			}
		}
		if (!PlayerPrefs.HasKey("Setting2"))
		{
			if (!Application.isMobilePlatform)
			{
				PlayerPrefs.SetInt("Setting2", 0);
			}
			else
			{
				PlayerPrefs.SetInt("Setting2", 1);
			}
		}
		if (!PlayerPrefs.HasKey("Setting3"))
		{
			PlayerPrefs.SetInt("Setting3", 0);
		}
		SceneManager.LoadScene("Menu");
	}
}
