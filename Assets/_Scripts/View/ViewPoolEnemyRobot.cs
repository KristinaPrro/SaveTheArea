using ModestTree;
using UnityEngine;

public abstract class ViewPoolEnemyRobot : ViewPool
{
	private Rigidbody2D _rigidbody;
	private TriggerComponent _trigger;
	private Collider2D _collider;

	[field: SerializeField]
	public AnimationComponent AnimationComponent { get; private set; }

	[field: SerializeField]
	public ParticleSystem ParticleSystemDie { get; private set; }
	[field: SerializeField]
	public ViewSlider2D ViewSlider { get; private set; }

	public Rigidbody2D Rigidbody
	{
		get
		{
			if (_rigidbody == null)
				_rigidbody = GetComponent<Rigidbody2D>();

			return _rigidbody;
		}
	}

	public Collider2D Collider
	{
		get
		{
			if (_collider == null)
				_collider = GetComponent<Collider2D>();

			return _collider;
		}
	}

	public TriggerComponent Trigger
	{
		get
		{
			if (_trigger == null)
				_trigger = GetComponent<TriggerComponent>();

			return _trigger;
		}
	}

	public void Awake()
	{
		Assert.IsNotNull(AnimationComponent);
		Assert.IsNotNull(ParticleSystemDie);
		Assert.IsNotNull(ViewSlider);
	}
}