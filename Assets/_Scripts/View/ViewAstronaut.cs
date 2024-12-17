using ModestTree;
using UnityEngine;

public class ViewAstronaut : View
{
	[field: SerializeField]
	public Rigidbody2D Rigidbody { get; private set; }
	[field: SerializeField]
	public Transform ContainerWeapon { get; private set; }
	[field: SerializeField]
	public Transform ContainerBullet { get; private set; }
	[field: SerializeField]
	public AnimationComponent AnimationComponent { get; private set; }
	[field: SerializeField]
	public Collider2D Collider { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Rigidbody);
		Assert.IsNotNull(ContainerWeapon);
		Assert.IsNotNull(AnimationComponent);
		Assert.IsNotNull(ContainerBullet);
		Assert.IsNotNull(Collider);
	}
}