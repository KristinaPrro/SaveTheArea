using ModestTree;
using UnityEngine;

public class ViewFinishLine : View
{
	[field: SerializeField]
	public Collider2D Collider { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Collider);
	}
}