using System;
using UniRx;
using UniRx.Triggers;
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
	private int CharacterDamagePerShot => 1;//todo

	private Rigidbody2D Rigidbody => View.Rigidbody;

	public int Id { get; private set; }
	public IObservable<int> HealthStream => _health;
	public int Health => _health.Value;

	public PresenterPoolEnemyRobotBase(TView view, SignalBus signalBus) : base(view)
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

	public void SetEnemyData(float speed, int health, int id)
	{
		_speed = speed;
		_health.Value = health;
		Id = id;

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
		_signalBus.Fire(new SignalPlayerDamage());
		//fx, destroy
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.FINISH_TAG:
				Explode();
				break;
			case ObjectUtils.BULLET_TAG:
				SetDamage(CharacterDamagePerShot); //todo
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