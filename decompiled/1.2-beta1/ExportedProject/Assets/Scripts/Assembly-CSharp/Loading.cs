using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	private HttpClient httpClient = new HttpClient();

	private string clientVersion = "1.1.1";

	private bool shouldLoadUpdateScene;

	private bool versionChecked;

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
	}

	private void Start()
	{
		try
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				versionChecked = true;
				shouldLoadUpdateScene = false;
			}
			else
			{
				httpClient.GetAsync("https://foodiedash.xytriza.com/api/getLatestVersion.php").ContinueWith(delegate(Task<HttpResponseMessage> responseTask)
				{
					if (responseTask.IsCompletedSuccessfully)
					{
						versionChecked = true;
						HttpResponseMessage result = responseTask.Result;
						if (result.IsSuccessStatusCode && result.Content.ReadAsStringAsync().Result != clientVersion)
						{
							shouldLoadUpdateScene = true;
						}
					}
					else
					{
						GameObject obj2 = GameObject.Find("LoadingText");
						obj2.GetComponent<TextMesh>().text = "Error loading. Please contact support";
						obj2.GetComponent<TextMesh>().fontSize = 50;
					}
				});
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			GameObject obj = GameObject.Find("LoadingText");
			obj.GetComponent<TextMesh>().text = "Error loading. Please contact support";
			obj.GetComponent<TextMesh>().fontSize = 50;
		}
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		else
		{
			Screen.fullScreen = PlayerPrefs.GetInt("Setting1") == 1;
		}
	}

	private void Update()
	{
		if (versionChecked)
		{
			if (shouldLoadUpdateScene)
			{
				SceneManager.LoadScene("UpdateRequired");
				shouldLoadUpdateScene = false;
			}
			else
			{
				SceneManager.LoadScene("Menu");
			}
		}
	}

	private static bool IsNewerVersion(string currentVersion, string latestVersion)
	{
		if (Version.TryParse(currentVersion, out var result) && Version.TryParse(latestVersion, out var result2))
		{
			return result2 > result;
		}
		return false;
	}
}
