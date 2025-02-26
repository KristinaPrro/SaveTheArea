using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	[field: SerializeField] // todo delete
	public int Id { get; private set; } = ObjectUtils.DEFAULT_ID;
	[field: SerializeField] // todo delete
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