using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Iconkit : MonoBehaviour
{
	public GameObject panel;

	public GameObject iconSelectionPanel;

	public GameObject overlaySelectionPanel;

	public Button backButton;

	private void Start()
	{
		backButton.onClick.AddListener(GoToMenu);
		iconSelectionPanel.SetActive(value: true);
		overlaySelectionPanel.SetActive(value: false);
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
