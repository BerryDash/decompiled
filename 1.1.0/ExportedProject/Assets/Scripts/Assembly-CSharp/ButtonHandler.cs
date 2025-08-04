using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	private void Awake()
	{
		GameObject.Find("Canvas/Button").GetComponent<Button>().onClick.AddListener(TaskOnClick);
	}

	private void TaskOnClick()
	{
		string text = "/windows";
		Application.OpenURL("https://foodiedash.xytriza.com/download" + text);
	}
}
