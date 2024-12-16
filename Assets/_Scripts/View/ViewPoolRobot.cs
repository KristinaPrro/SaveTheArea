using ModestTree;
using UnityEngine;

public abstract class ViewPoolRobot : ViewPool
{
	[field: SerializeField]
	public Rigidbody2D Rigidbody { get; private set; }
	[field: SerializeField]
	public AnimationComponent AnimationComponent { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Rigidbody);
		Assert.IsNotNull(AnimationComponent);
	}
}