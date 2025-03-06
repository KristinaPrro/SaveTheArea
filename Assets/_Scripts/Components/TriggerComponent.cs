using UnityEngine;

public class TriggerComponent : MonoBehaviour, ITriggerComponent
{
	[field: SerializeField]
	private string _myLayer = ObjectUtils.IGNOR_COLLISIONS_LAYER;
	
	[field: SerializeField]
	public int Id { get; private set; } = ObjectUtils.DEFAULT_ID;

	public int MyLayerId => LayerMask.NameToLayer(_myLayer);
	public bool IsVisible => MyLayerId == MyLayerId && MyLayerId != ObjectUtils.IGNOR_COLLISIONS_LAYER_ID;

	public void SetId(int id)
	{
		Id = id;
	}

	public void SetVisible(bool isVisible)
	{
		gameObject.layer = isVisible ? MyLayerId : ObjectUtils.IGNOR_COLLISIONS_LAYER_ID;
	}
}