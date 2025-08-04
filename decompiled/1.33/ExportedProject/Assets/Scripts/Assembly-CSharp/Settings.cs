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

	public Toggle setting1secrettoggle;

	public Button backbutton;

	public Slider musicSlider;

	public Slider sfxSlider;

	private void Awake()
	{
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
		sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
		if (!Application.isMobilePlatform)
		{
			setting1toggle.isOn = PlayerPrefs.GetInt("Setting1", 1) == 1;
			setting2toggle.isOn = PlayerPrefs.GetInt("Setting2", 0) == 1;
			setting3toggle.isOn = PlayerPrefs.GetInt("Setting3", 0) == 1;
			setting4toggle.isOn = PlayerPrefs.GetInt("Setting4", 0) == 1;
			setting5toggle.isOn = PlayerPrefs.GetInt("Setting5", 1) == 1;
			setting3toggle.interactable = PlayerPrefs.GetInt("Setting2", 0) == 1;
		}
		else
		{
			setting1toggle.isOn = true;
			setting2toggle.isOn = true;
			setting3toggle.isOn = PlayerPrefs.GetInt("Setting3", 0) == 1;
			setting4toggle.isOn = PlayerPrefs.GetInt("Setting4", 0) == 1;
			setting5toggle.isOn = true;
			setting1toggle.interactable = false;
			setting2toggle.interactable = false;
			setting5toggle.interactable = false;
		}
		setting1secrettoggle.isOn = PlayerPrefs.GetInt("secretSetting1", 0) == 1;
		setting1secrettoggle.gameObject.SetActive(PlayerPrefs.GetInt("userID", 0) == 1);
		backbutton.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Menu");
		});
		setting1toggle.onValueChanged.AddListener(delegate
		{
			Screen.fullScreen = setting1toggle.isOn;
			PlayerPrefs.SetInt("Setting1", setting1toggle.isOn ? 1 : 0);
		});
		setting2toggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("Setting2", setting2toggle.isOn ? 1 : 0);
			setting3toggle.interactable = setting2toggle.isOn;
			setting3toggle.isOn = setting2toggle.isOn && setting3toggle.isOn;
			PlayerPrefs.SetInt("Setting3", setting3toggle.isOn ? 1 : 0);
		});
		setting3toggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("Setting3", setting3toggle.isOn ? 1 : 0);
		});
		setting4toggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("Setting4", setting4toggle.isOn ? 1 : 0);
		});
		setting5toggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("Setting5", setting5toggle.isOn ? 1 : 0);
			QualitySettings.vSyncCount = (setting5toggle.isOn ? 1 : 0);
		});
		setting1secrettoggle.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetInt("secretSetting1", setting1secrettoggle.isOn ? 1 : 0);
		});
		musicSlider.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
			PlayerPrefs.Save();
		});
		sfxSlider.onValueChanged.AddListener(delegate
		{
			PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
			PlayerPrefs.Save();
		});
	}
}
