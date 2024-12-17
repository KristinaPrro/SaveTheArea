using ModestTree;
using UnityEngine;

public abstract class ViewPoolEnemyRobot : ViewPool
{
	[field: SerializeField]
	public Rigidbody2D Rigidbody { get; private set; }
	[field: SerializeField]
	public AnimationComponent AnimationComponent { get; private set; }
	[field: SerializeField]
	public Collider2D Collider { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Rigidbody);
		Assert.IsNotNull(AnimationComponent);
		Assert.IsNotNull(Collider);
	}
}