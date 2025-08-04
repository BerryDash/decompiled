using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
	public Button backButton;

	public Button leaderboardsButton;

	private void Awake()
	{
		leaderboardsButton.onClick.AddListener(LeaderboardsButton);
		backButton.onClick.AddListener(BackButton);
	}

	private void LeaderboardsButton()
	{
		Application.OpenURL("https://berrydash.xytriza.com/leaderboard.php");
	}

	private void BackButton()
	{
		SceneManager.LoadScene("Menu");
	}
}
