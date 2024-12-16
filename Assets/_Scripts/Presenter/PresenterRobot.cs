using System;
using UniRx;
using UnityEngine;
using Zenject;

public class PresenterRobot : Presenter, IEnemy
{
	private readonly ReactiveProperty<int> _health;
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private float _speed;

	public IObservable<int> HealthStream => _health;
	public int Health => _health.Value;

	public PresenterRobot(EnemyData enemyData, SignalBus signalBus)
	{
		_speed = enemyData.Speed;
		_health.Value = enemyData.MaxHealth;

		_signalBus = signalBus;
	}

	public override void Initialize()
	{
		
	}

	public override void Dispose()
	{
		base.Dispose();

		_disposables.Dispose();
		StopMoving();
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