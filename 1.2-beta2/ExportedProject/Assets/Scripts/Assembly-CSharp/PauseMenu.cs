using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	private GameObject pauseMenuObj;

	private void Start()
	{
		pauseMenuObj = GameObject.Find("Canvas/PauseMenu");
		GameObject obj = GameObject.Find("Canvas/PauseMenu/ReturnToMenu");
		GameObject gameObject = GameObject.Find("Canvas/PauseMenu/ReturnToMenu");
		GameObject gameObject2 = GameObject.Find("Canvas/PauseMenu/ReturnToMenu");
		Button component = obj.GetComponent<Button>();
		Button component2 = gameObject.GetComponent<Button>();
		Button component3 = gameObject2.GetComponent<Button>();
		component.onClick.AddListener(MenuClick);
		component2.onClick.AddListener(TogglePauseMenu);
		component3.onClick.AddListener(SettingsClick);
	}

	public void TogglePauseMenu()
	{
		pauseMenuObj.SetActive(value: false);
	}

	private void MenuClick()
	{
		SceneManager.LoadScene("Game");
	}

	private void SettingsClick()
	{
		pauseMenuObj.SetActive(!pauseMenuObj.activeSelf);
	}
}
