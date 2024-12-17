using ModestTree;
using UnityEngine;

public abstract class ViewPoolDamageElement : ViewPool
{
	[field: SerializeField]
	public Rigidbody2D Rigidbody { get; private set; }
	[field: SerializeField]
	public Collider2D Collider { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Rigidbody);
		Assert.IsNotNull(Collider);
	}
}
