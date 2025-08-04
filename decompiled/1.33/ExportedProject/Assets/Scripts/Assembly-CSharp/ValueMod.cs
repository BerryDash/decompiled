using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueMod : MonoBehaviour
{
	public TMP_InputField ValueToMod;

	public TMP_InputField Value;

	public TMP_Dropdown ModType;

	public Button submit;

	private int modType;

	private void Start()
	{
		ValueToMod.onValueChanged.AddListener(delegate
		{
			ValueToModInput();
		});
		ModType.onValueChanged.AddListener(delegate
		{
			modType = ModType.value;
			ValueToModInput();
		});
		submit.onClick.AddListener(delegate
		{
			string text = ValueToMod.text;
			if (modType == 0)
			{
				PlayerPrefs.SetString(text, Value.text);
			}
			else if (modType == 1)
			{
				PlayerPrefs.SetFloat(text, float.Parse(Value.text));
			}
			else if (modType == 2)
			{
				PlayerPrefs.SetInt(text, int.Parse(Value.text));
			}
		});
	}

	private void ValueToModInput()
	{
		string text = ValueToMod.text;
		if (modType == 0)
		{
			Value.text = PlayerPrefs.GetString(text, "");
		}
		else if (modType == 1)
		{
			Value.text = PlayerPrefs.GetFloat(text, 0f).ToString();
		}
		else if (modType == 2)
		{
			Value.text = PlayerPrefs.GetInt(text, 0).ToString();
		}
	}
}
