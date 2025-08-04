using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	private void Awake()
	{
		GameObject obj = GameObject.Find("Canvas/PlayButton");
		GameObject gameObject = GameObject.Find("Canvas/SettingsButton");
		Button component = obj.GetComponent<Button>();
		Button component2 = gameObject.GetComponent<Button>();
		component.onClick.AddListener(PlayClick);
		component2.onClick.AddListener(SettingsClick);
	}

	private void PlayClick()
	{
		SceneManager.LoadScene("Game");
	}

	private void SettingsClick()
	{
		SceneManager.LoadScene("Settings");
	}
}
