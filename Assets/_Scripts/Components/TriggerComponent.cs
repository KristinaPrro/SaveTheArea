using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	[field: SerializeField]
	public int Id { get; private set; } = ObjectUtils.DEFAULT_ID;
	[field: SerializeField]
	public bool IsVisible { get; private set; }

	public void SetId(int id)
	{
		Id = id;
	}

	public void SetVisible(bool isVisible)
	{
		IsVisible = isVisible;
	}
}