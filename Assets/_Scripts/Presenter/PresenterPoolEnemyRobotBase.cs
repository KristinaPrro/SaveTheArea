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
	private readonly EnemyData _startEnemyData;

	private Vector2 _directionMovement;
	private Rigidbody2D Rigidbody => View.Rigidbody;

	public int Id => _startEnemyData.Id;
	public float Speed => _startEnemyData.Speed;
	public Vector2 DirectionMovement => _directionMovement;
	public Transform TransformPosition => View.transform;
	public IObservable<int> HealthStream => _health;
	public int Health => _health.Value;

	public PresenterPoolEnemyRobotBase(TView view, EnemyData enemyData, SignalBus signalBus) : base(view)
	{
		_signalBus = signalBus;
		_startEnemyData = enemyData;
		View.Trigger.SetId(enemyData.Id);
	}

	public override void Initialize()
	{
		base.Initialize();

		_health.Value = _startEnemyData.Health;

		View.transform.position = _startEnemyData.StartPosition.position;
		OnDirectionChange(Vector2.down);
	}

	public override void Dispose()
	{
		_disposables.Dispose();
		StopMoving();

		base.Dispose();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * _startEnemyData.Speed * Time.deltaTime);
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