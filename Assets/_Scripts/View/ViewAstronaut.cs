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
	public CircleCollider2D CircleCollider { get; private set; }

	public Vector3 StartPosition { get; private set; }

	public void Awake()
	{
		Assert.IsNotNull(Rigidbody);
		Assert.IsNotNull(ContainerWeapon);
		Assert.IsNotNull(AnimationComponent);
		Assert.IsNotNull(ContainerBullet);
		Assert.IsNotNull(CircleCollider);

		StartPosition = Rigidbody.transform.position;
	}
}