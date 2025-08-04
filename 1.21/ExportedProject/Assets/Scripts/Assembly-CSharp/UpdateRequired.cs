using UnityEngine;
using UnityEngine.UI;

public class UpdateRequired : MonoBehaviour
{
	public Button btn;

	private void Awake()
	{
		btn.onClick.AddListener(TaskOnClick);
	}

	private void TaskOnClick()
	{
		string text = "/windows";
		Application.OpenURL("https://berrydash.xytriza.com/download" + text);
	}
}
