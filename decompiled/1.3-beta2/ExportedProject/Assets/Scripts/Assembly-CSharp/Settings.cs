using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public Toggle setting1toggle;

	public Toggle setting2toggle;

	public Toggle setting3toggle;

	public Toggle setting4toggle;

	public Toggle setting5toggle;

	public Button backbutton;

	public Slider musicSlider;

	public Slider sfxSlider;

	private void Awake()
	{
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
		sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
		setting1toggle.isOn = PlayerPrefs.GetInt("Setting1", 1) == 1;
		setting2toggle.isOn = PlayerPrefs.GetInt("Setting2", 0) == 1;
		setting3toggle.isOn = PlayerPrefs.GetInt("Setting3", 0) == 1;
		setting4toggle.isOn = PlayerPrefs.GetInt("Setting4", 0) == 1;
		setting5toggle.isOn = PlayerPrefs.GetInt("Setting5", 1) == 1;
		backbutton.onClick.AddListener(BackButton);
		setting1toggle.onValueChanged.AddListener(delegate
		{
			Setting1ToggleValueChanged(setting1toggle);
		});
		setting2toggle.onValueChanged.AddListener(delegate
		{
			Setting2ToggleValueChanged(setting2toggle);
		});
		setting3toggle.onValueChanged.AddListener(delegate
		{
			Setting3ToggleValueChanged(setting3toggle);
		});
		setting4toggle.onValueChanged.AddListener(delegate
		{
			Setting4ToggleValueChanged(setting4toggle);
		});
		setting5toggle.onValueChanged.AddListener(delegate
		{
			Setting5ToggleValueChanged(setting5toggle);
		});
		musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
		sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
	}

	private void Setting1ToggleValueChanged(Toggle change)
	{
		Screen.fullScreen = change.isOn;
		PlayerPrefs.SetInt("Setting1", change.isOn ? 1 : 0);
	}

	private void Setting2ToggleValueChanged(Toggle change)
	{
		PlayerPrefs.SetInt("Setting2", change.isOn ? 1 : 0);
	}

	private void Setting3ToggleValueChanged(Toggle change)
	{
		PlayerPrefs.SetInt("Setting3", change.isOn ? 1 : 0);
	}

	private void Setting4ToggleValueChanged(Toggle change)
	{
		PlayerPrefs.SetInt("Setting4", change.isOn ? 1 : 0);
	}

	private void Setting5ToggleValueChanged(Toggle change)
	{
		PlayerPrefs.SetInt("Setting5", change.isOn ? 1 : 0);
		QualitySettings.vSyncCount = (change.isOn ? 1 : 0);
	}

	private void BackButton()
	{
		SceneManager.LoadScene("Menu");
	}

	private void UpdateMusicVolume(float volume)
	{
		PlayerPrefs.SetFloat("musicVolume", volume);
		PlayerPrefs.Save();
	}

	private void UpdateSFXVolume(float volume)
	{
		PlayerPrefs.SetFloat("sfxVolume", volume);
		PlayerPrefs.Save();
	}
}
