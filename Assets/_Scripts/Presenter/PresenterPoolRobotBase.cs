using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PresenterPoolRobotBase<TView> : PresenterPoolBase<TView>, IEnemy
	 where TView : ViewPoolRobot
{
	private readonly ReactiveProperty<int> _health;
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private float _speed;

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
	}

	public void SetDamage(int damage)
	{
		_health.Value -= damage;
	}

	public void Explode()
	{
		_signalBus.Fire(new SignalPlayerDamage());
		//fx, destroy
	}

	public void Move(Vector2 vector)
	{

	}

	public void StopMoving()
	{

	}
}