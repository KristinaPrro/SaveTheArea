using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PresenterPoolRobotBase<TView> : PresenterPoolBase<TView>, IEnemy, ITickable
	 where TView : ViewPoolRobot
{
	private readonly ReactiveProperty<int> _health = new();
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private float _speed;
	private Vector2 _directionMovement;

	private Rigidbody2D Rigidbody => View.Rigidbody;

	public PresenterPoolRobotBase(TView view, SignalBus signalBus) : base(view)
	{
		_signalBus = signalBus;
	}

	public IObservable<int> HealthStream => _health;
	public int Health => _health.Value;

	public override void Initialize()
	{
		//trigger
	}

	public override void Dispose()
	{
		base.Dispose();

		_disposables.Dispose();
		StopMoving();
	}

	public void SetEnemyData(float speed, int health)
	{
		_speed = speed;
		_health.Value = health;

		Move(Vector2.down);
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * _speed * Time.deltaTime);
	}

	public void SetDamage(int damage)
	{
		_health.Value -= damage;

		if (_health.Value <= 0)
			Die();
	}

	public void Explode()
	{
		_signalBus.Fire(new SignalPlayerDamage());
		Die();
		//fx, destroy
	}

	public void Move(Vector2 direction)
	{
		View.AnimationComponent.Move(direction);
		_directionMovement = direction;
	}

	public void StopMoving()
	{
		View.AnimationComponent.StopMoving();
		_directionMovement = Vector2.zero;
	}

	private void Die()
	{
		_directionMovement = Vector2.zero;
		View.AnimationComponent.Die();
	}
}