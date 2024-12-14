using UnityEngine;

public class UiView : MonoBehaviour
{
	public virtual void Show(bool isShow = true)
	{
		gameObject.SetActive(isShow);
	}

	public virtual void Hide()
	{
		Show(false);
	}
}