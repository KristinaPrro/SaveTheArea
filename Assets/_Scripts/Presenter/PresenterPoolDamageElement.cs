using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public abstract class PresenterPoolDamageElement<TView> : PresenterPoolBase<TView>, ITickable, IDamageElement
	 where TView : ViewPoolDamageElement
{
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private Vector2 _directionMovement;

	protected int Damage { get; set; }
	protected float Speed { get; set; }
	protected Rigidbody2D Rigidbody => View.Rigidbody;

	public int Id { get; private set; }

	public PresenterPoolDamageElement(TView view, SignalBus signalBus) : base(view)
	{
		_signalBus = signalBus;
	}

	public override void Initialize()
	{
		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter2D).AddTo(_disposables);
	}

	public override void Dispose()
	{
		base.Dispose();

		_disposables.Dispose();
		StopMoving();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * Speed * Time.deltaTime);
	}

	public void SetData(float speed, int damage, int id)
	{
		Speed = speed;
		Damage = damage;
		Id = id;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:
				//todo
				break;
		}
	}

	public void OnDirectionChange(Vector2 direction)
	{
		_directionMovement = direction;
	}

	public void StopMoving()
	{
		_directionMovement = Vector2.zero;
	}

	public void Disappear() //todo
	{
		_directionMovement = Vector2.zero;
		//View.SelfRelease();
	}
}