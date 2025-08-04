using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public GameObject pausePanel;

	public Button menuButton;

	public Button playButton;

	public AudioSource backgroundMusic;

	public Slider musicSlider;

	public Slider sfxSlider;

	private void Start()
	{
		menuButton.onClick.AddListener(MenuClick);
		playButton.onClick.AddListener(TogglePause);
		musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
		sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
		sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
	}

	private void TogglePause()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		backgroundMusic.Play();
		pausePanel.SetActive(value: false);
	}

	private void MenuClick()
	{
		SceneManager.LoadScene("Menu");
	}

	private void UpdateMusicVolume(float volume)
	{
		PlayerPrefs.SetFloat("musicVolume", volume);
		PlayerPrefs.Save();
		backgroundMusic.volume = volume;
	}

	private void UpdateSFXVolume(float volume)
	{
		PlayerPrefs.SetFloat("sfxVolume", volume);
		PlayerPrefs.Save();
	}
}
