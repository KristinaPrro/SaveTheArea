using System;
using UniRx;
using UnityEngine;
using Zenject;

public abstract class PresenterPoolEnemyRobotBase<TView> : PresenterPoolBase<TView>, IEnemy, ITickable
	 where TView : ViewPoolEnemyRobot
{
	private readonly ReactiveProperty<int> _health = new();
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private float _speed;
	private Vector2 _directionMovement;
	private Rigidbody2D Rigidbody => View.Rigidbody;

	public int Id { get; private set; }
	public float Speed => _speed;
	public Vector2 DirectionMovement => _directionMovement;
	public Vector2 Position => View.transform.position;
	public IObservable<int> HealthStream => _health;
	public int Health => _health.Value;

	public PresenterPoolEnemyRobotBase(TView view, SignalBus signalBus) : base(view)
	{
		_signalBus = signalBus;
	}

	public override void Dispose()
	{
		_disposables.Dispose();
		StopMoving();

		base.Dispose();
	}

	public void SetEnemyData(float speed, int health, int id)
	{
		_speed = speed;
		_health.Value = health;

		Id = id;
		View.Trigger.SetId(id);

		OnDirectionChange(Vector2.down);
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * _speed * Time.deltaTime);
	}

	public void OnDirectionChange(Vector2 direction)
	{
		View.AnimationComponent.Move(direction);
		_directionMovement = direction;
	}

	public void StopMoving()
	{
		View.AnimationComponent.StopMoving();
		_directionMovement = Vector2.zero;
	}

	public void SetDamage(int damage)
	{
		_health.Value -= damage;

		if (_health.Value <= 0)
			Die();
	}

	public void Explode()//todo
	{
		//fx, destroy
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.FINISH_TAG:
				Explode();
				break;
		}
	}

	private void Die() //todo
	{
		_directionMovement = Vector2.zero;
		//View.SelfRelease();
		View.AnimationComponent.Die();
	}
}