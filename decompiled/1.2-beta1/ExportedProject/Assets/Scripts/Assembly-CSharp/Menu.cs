using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public RuntimePlatform[] desktopPlatforms = new RuntimePlatform[3]
	{
		RuntimePlatform.WindowsPlayer,
		RuntimePlatform.LinuxPlayer,
		RuntimePlatform.OSXPlayer
	};

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("Canvas/ExitButton");
		GameObject gameObject2 = GameObject.Find("Canvas/PlayButton");
		GameObject gameObject3 = GameObject.Find("Canvas/SettingsButton");
		Button component = gameObject.GetComponent<Button>();
		Button component2 = gameObject2.GetComponent<Button>();
		Button component3 = gameObject3.GetComponent<Button>();
		if (desktopPlatforms.Contains(Application.platform))
		{
			component.onClick.AddListener(ExitClick);
			component2.onClick.AddListener(PlayClick);
			component3.onClick.AddListener(SettingsClick);
		}
		else
		{
			Object.Destroy(gameObject);
			gameObject2.transform.localPosition = new Vector3(-80f, 0f, 0f);
			gameObject3.transform.localPosition = new Vector3(80f, 0f, 0f);
		}
	}

	private void PlayClick()
	{
		SceneManager.LoadScene("Game");
	}

	private void SettingsClick()
	{
		SceneManager.LoadScene("Settings");
	}

	private void ExitClick()
	{
		Application.Quit();
	}
}
