using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Iconkit : MonoBehaviour
{
	public GameObject panel;

	public GameObject iconSelectionPanel;

	public GameObject overlaySelectionPanel;

	public Button backButton;

	public Image defaultIcon;

	public Button placeholderButton;

	public TMP_Text selectionText;

	public Image previewBird;

	public Image previewOverlay;

	public Image overlay0;

	public Image overlay1;

	public Image overlay2;

	public Image overlay3;

	public Image overlay4;

	public Image overlay5;

	public Image overlay6;

	public Image overlay7;

	public Image overlay8;

	public Image overlay9;

	public Image icon1;

	public Image icon2;

	public Image icon3;

	public Image icon4;

	public Image icon5;

	public Image icon6;

	public Image icon7;

	public Image icon8;

	public GameObject previewBirdObject;

	private void Start()
	{
		SwitchToIcon();
		SelectOverlay(PlayerPrefs.GetInt("overlay", Mathf.Clamp(PlayerPrefs.GetInt("overlay", 0), 0, 9)));
		SelectIcon(PlayerPrefs.GetInt("icon", Mathf.Clamp(PlayerPrefs.GetInt("icon", 0), 1, 8)));
		if (PlayerPrefs.GetInt("icon", 0) == 7)
		{
			SelectOverlay(0);
			placeholderButton.interactable = false;
		}
		if (PlayerPrefs.GetInt("userID", 0) == 1)
		{
			defaultIcon.sprite = Resources.Load<Sprite>("icons/icons/bird_-1");
		}
		else if (PlayerPrefs.GetInt("userID", 0) == 2)
		{
			defaultIcon.sprite = Resources.Load<Sprite>("icons/icons/bird_-2");
		}
		else if (PlayerPrefs.GetInt("userID", 0) == 4)
		{
			defaultIcon.sprite = Resources.Load<Sprite>("icons/icons/bird_-3");
		}
		placeholderButton.onClick.AddListener(ToggleKit);
		backButton.onClick.AddListener(delegate
		{
			PlayerPrefs.SetInt("icon", Mathf.Clamp(PlayerPrefs.GetInt("icon", 0), 1, 8));
			PlayerPrefs.SetInt("overlay", Mathf.Clamp(PlayerPrefs.GetInt("overlay", 0), 0, 9));
			PlayerPrefs.Save();
			SceneManager.LoadScene("Menu");
		});
	}

	private void FixedUpdate()
	{
		if (!Input.GetMouseButton(0))
		{
			return;
		}
		Vector2 screenPoint = Input.mousePosition;
		Image[] array = new Image[10] { overlay0, overlay1, overlay2, overlay3, overlay4, overlay5, overlay6, overlay7, overlay8, overlay9 };
		Image[] array2 = new Image[8] { icon1, icon2, icon3, icon4, icon5, icon6, icon7, icon8 };
		for (int i = 0; i < array.Length; i++)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(array[i].rectTransform, screenPoint) && array[i].IsActive())
			{
				SelectOverlay(i);
				break;
			}
		}
		for (int j = 0; j < array2.Length; j++)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(array2[j].rectTransform, screenPoint) && array2[j].IsActive())
			{
				SelectIcon(j + 1);
				break;
			}
		}
		if (RectTransformUtility.RectangleContainsScreenPoint(previewBirdObject.GetComponent<RectTransform>(), screenPoint))
		{
			float x = previewBirdObject.transform.localScale.x;
			previewBirdObject.transform.localScale = new Vector3((x != 1f) ? 1 : (-1), 1f, 1f);
		}
	}

	private void SwitchToIcon()
	{
		iconSelectionPanel.SetActive(value: true);
		overlaySelectionPanel.SetActive(value: false);
		selectionText.text = "Icon selection";
		placeholderButton.GetComponentInChildren<TMP_Text>().text = "Overlays";
	}

	private void SwitchToOverlay()
	{
		iconSelectionPanel.SetActive(value: false);
		overlaySelectionPanel.SetActive(value: true);
		selectionText.text = "Overlay selection";
		placeholderButton.GetComponentInChildren<TMP_Text>().text = "Icons";
	}

	private void ToggleKit()
	{
		if (GetCurrentKit() == 1)
		{
			SwitchToOverlay();
		}
		else if (GetCurrentKit() == 2)
		{
			SwitchToIcon();
		}
	}

	private int GetCurrentKit()
	{
		if (iconSelectionPanel.activeSelf)
		{
			return 1;
		}
		if (overlaySelectionPanel.activeSelf)
		{
			return 2;
		}
		return 0;
	}

	private void SelectIcon(int iconID)
	{
		PlayerPrefs.SetInt("icon", iconID);
		Image[] array = new Image[8] { icon1, icon2, icon3, icon4, icon5, icon6, icon7, icon8 };
		Color32 color = new Color32(70, 70, 70, byte.MaxValue);
		Color32 color2 = new Color32(100, 100, 100, byte.MaxValue);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = ((iconID == i + 1) ? color : color2);
		}
		previewBird.sprite = Resources.Load<Sprite>("icons/icons/bird_" + iconID);
		if (iconID == 1)
		{
			if (PlayerPrefs.GetInt("userID", 0) == 1)
			{
				previewBird.sprite = Resources.Load<Sprite>("icons/icons/bird_-1");
			}
			else if (PlayerPrefs.GetInt("userID", 0) == 2)
			{
				previewBird.sprite = Resources.Load<Sprite>("icons/icons/bird_-2");
			}
			else if (PlayerPrefs.GetInt("userID", 0) == 4)
			{
				previewBird.sprite = Resources.Load<Sprite>("icons/icons/bird_-3");
			}
		}
		if (iconID == 7)
		{
			SelectOverlay(0);
			placeholderButton.interactable = false;
		}
		else
		{
			placeholderButton.interactable = true;
		}
	}

	private void SelectOverlay(int overlayID)
	{
		PlayerPrefs.SetInt("overlay", overlayID);
		Image[] array = new Image[10] { overlay0, overlay1, overlay2, overlay3, overlay4, overlay5, overlay6, overlay7, overlay8, overlay9 };
		Color32 color = new Color32(70, 70, 70, byte.MaxValue);
		Color32 color2 = new Color32(100, 100, 100, byte.MaxValue);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = ((overlayID == i) ? color : color2);
		}
		previewOverlay.rectTransform.localPosition = new Vector3(-19f, 28f, 0f);
		previewOverlay.gameObject.SetActive(value: true);
		if (overlayID == 8)
		{
			previewOverlay.rectTransform.localPosition = new Vector3(-22f, 20f, 0f);
		}
		if (overlayID == 0)
		{
			previewOverlay.gameObject.SetActive(value: false);
			previewOverlay.sprite = null;
		}
		else
		{
			previewOverlay.sprite = Resources.Load<Sprite>("icons/overlays/overlay_" + overlayID);
		}
	}
}
