using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSocials : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public string URL;

	public void OnPointerClick(PointerEventData eventData)
	{
		Application.OpenURL(URL);
	}
}
