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
		_disposables.Dispose();
		StopMoving();

		base.Dispose();
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

				if (!other.TryGetComponent<ITriggerComponent>(out var enemy))
				{
					this.LogError($"{nameof(ITriggerComponent)} component not found on object with {other.tag} tag!");
					break;
				}

				_signalBus.Fire(new SignalEnemyDamage(Id, enemy.Id, Damage));
				break;

			case ObjectUtils.DISAPPEARANCE_TAG:

				_signalBus.Fire(new SignalDisappearanceDamageElement(Id));
				break;
		}
	}

	public void SetTarget(Transform targetPosition,
		float speed,
		Vector2 targetDirectionMovement,
		Transform startPosition)
	{
		Vector2 startTargetPosition = targetPosition.position;
		var acceptableError = targetPosition.lossyScale.y / 2;
		var sumSpeed = Speed + speed;

		var shiftTargetPosition = startTargetPosition;
		var meetPosition = startTargetPosition;

		do
		{
			meetPosition = shiftTargetPosition;
			var distance = Vector2.Distance(meetPosition, startTargetPosition) 
				+ Vector2.Distance(meetPosition, startPosition.position);

			var time = distance / sumSpeed;
			shiftTargetPosition = startTargetPosition + targetDirectionMovement * speed * time;

			//shiftTargetPosition = meetPosition + targetDirectionMovement * speed * time;
			//meetPosition += targetDirectionMovement * speed * time;
			//new Vector2(targetPosition.position.y - speed * time, targetPosition.position.x);
			//meetPosition = new Vector2(targetPosition.position.y - speed * time, targetPosition.position.x);
			//targetWayDistance = (Vector2)startPosition.position + targetDirectionMovement * speed * time;
		}
		while (Vector2.Distance(meetPosition, shiftTargetPosition) < acceptableError);

		_directionMovement = Vector3.ClampMagnitude(shiftTargetPosition - (Vector2)startPosition.position, 1);
		
		View.transform.position = startPosition.position;
	}

	public void StopMoving()
	{
		_directionMovement = Vector2.zero;
	}
}