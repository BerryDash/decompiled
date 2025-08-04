using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
	public Canvas canvas;

	public ScrollRect scrollRect;

	public GameObject content;

	public TMP_Dropdown showAmount;

	public TMP_Text statusText;

	public TMP_FontAsset font;

	public Button backButton;

	public Button refreshButton;

	private void Awake()
	{
		scrollRect.normalizedPosition = new Vector2(0f, 1f);
		UpdateStatus(enabled: true, "Loading...");
		StartCoroutine(GetTopPlayers(0));
		showAmount.onValueChanged.AddListener(delegate(int value)
		{
			StartCoroutine(RefreshLeaderboard(value));
		});
		backButton.onClick.AddListener(delegate
		{
			SceneManager.LoadScene("Menu");
		});
		refreshButton.onClick.AddListener(delegate
		{
			StartCoroutine(RefreshLeaderboard(showAmount.value));
		});
	}

	private IEnumerator RefreshLeaderboard(int value)
	{
		int childCount = content.transform.childCount;
		GameObject[] entries = new GameObject[childCount];
		for (int i = 0; i < childCount; i++)
		{
			entries[i] = content.transform.GetChild(i).gameObject;
		}
		yield return StartCoroutine(GetTopPlayers(value));
		for (int j = 0; j < entries.Length; j++)
		{
			UnityEngine.Object.Destroy(entries[j]);
		}
	}

	private IEnumerator GetTopPlayers(int showAmount)
	{
		Debug.Log(showAmount);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("showAmount", showAmount);
		using UnityWebRequest request = UnityWebRequest.Post("https://berrydash.xytriza.com/database/getTopPlayers.php", wWWForm);
		request.SetRequestHeader("User-Agent", "BerryDashClient");
		request.SetRequestHeader("ClientVersion", PlayerPrefs.GetFloat("clientVersion").ToString());
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			UpdateStatus();
			string text = request.downloadHandler.text;
			Debug.Log(text);
			string[] array = text.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			int num = array.Length / 5;
			float num2 = 40f;
			float num3 = 10f;
			float num4 = content.GetComponent<RectTransform>().rect.height / 2f;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = new GameObject("Entry_" + i, typeof(RectTransform), typeof(CanvasRenderer));
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.sizeDelta = new Vector2(600f, num2);
				component.anchoredPosition = new Vector2(0f, num4 - (float)i * (num2 + num3));
				component.anchorMin = new Vector2(0.5f, 1f);
				component.anchorMax = new Vector2(0.5f, 1f);
				component.pivot = new Vector2(0.5f, 1f);
				GameObject gameObject2 = new GameObject("PlayerName", typeof(RectTransform), typeof(TextMeshProUGUI));
				GameObject gameObject3 = new GameObject("PlayerIcon", typeof(RectTransform), typeof(Image));
				GameObject gameObject4 = new GameObject("PlayerIconOverlay", typeof(RectTransform), typeof(Image));
				GameObject obj = new GameObject("Score", typeof(RectTransform), typeof(TextMeshProUGUI));
				gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
				gameObject3.transform.SetParent(gameObject2.transform, worldPositionStays: false);
				gameObject4.transform.SetParent(gameObject3.transform, worldPositionStays: false);
				obj.transform.SetParent(gameObject.transform, worldPositionStays: false);
				Image component2 = gameObject3.GetComponent<Image>();
				component2.sprite = Resources.Load<Sprite>("icons/icons/bird_" + array[i * 5 + 2]);
				if (array[i * 5 + 2] == "1")
				{
					if (array[i * 5 + 4] == "1")
					{
						component2.sprite = Resources.Load<Sprite>("icons/icons/bird_-1");
					}
					else if (array[i * 5 + 4] == "2")
					{
						component2.sprite = Resources.Load<Sprite>("icons/icons/bird_-2");
					}
					else if (array[i * 5 + 4] == "4")
					{
						component2.sprite = Resources.Load<Sprite>("icons/icons/bird_-3");
					}
					else if (array[i * 5 + 4] == "6")
					{
						component2.sprite = Resources.Load<Sprite>("icons/icons/bird_-4");
					}
				}
				component2.rectTransform.sizeDelta = new Vector2(30f, 30f);
				component2.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
				component2.rectTransform.anchoredPosition = new Vector2(-125f, 0f);
				Image component3 = gameObject4.GetComponent<Image>();
				if (array[i * 5 + 3] == "0")
				{
					gameObject4.SetActive(value: false);
				}
				if (array[i * 5 + 3] == "8")
				{
					component3.rectTransform.anchoredPosition = new Vector2(-8.25f, 7.5f);
				}
				else
				{
					component3.rectTransform.anchoredPosition = new Vector2(-7.125f, 10.5f);
				}
				component3.rectTransform.sizeDelta = new Vector2(13.125f, 5.625f);
				component3.sprite = Resources.Load<Sprite>("icons/overlays/overlay_" + array[i * 5 + 3]);
				TextMeshProUGUI component4 = gameObject2.GetComponent<TextMeshProUGUI>();
				component4.text = array[i * 5] + " (#" + (i + 1) + ")";
				component4.font = font;
				component4.fontSize = 14f;
				component4.alignment = TextAlignmentOptions.Left;
				component4.rectTransform.sizeDelta = new Vector2(200f, 75f);
				component4.rectTransform.anchoredPosition = new Vector2(-35f, 0f);
				TextMeshProUGUI component5 = obj.GetComponent<TextMeshProUGUI>();
				component5.text = "Score: " + array[i * 5 + 1];
				component5.font = font;
				component5.fontSize = 14f;
				component5.alignment = TextAlignmentOptions.Right;
				component5.rectTransform.sizeDelta = new Vector2(200f, 75f);
				component5.rectTransform.anchoredPosition = new Vector2(65f, 0f);
				gameObject.transform.SetParent(content.transform, worldPositionStays: false);
			}
			RectTransform component6 = content.GetComponent<RectTransform>();
			component6.sizeDelta = new Vector2(component6.sizeDelta.x, (float)num * (num2 + num3));
			yield break;
		}
		UpdateStatus(enabled: true, "Failed to fetch leaderboard stats");
		foreach (Transform item in content.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	private void UpdateStatus(bool enabled = false, string message = "")
	{
		statusText.gameObject.SetActive(enabled);
		statusText.text = message;
	}
}
