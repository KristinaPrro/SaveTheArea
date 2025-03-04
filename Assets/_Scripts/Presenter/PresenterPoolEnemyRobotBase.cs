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
	private readonly GameSettings _gameSettings;
	private readonly EnemyData _startEnemyData;

	private Vector2 _directionMovement;

	private Rigidbody2D Rigidbody => View.Rigidbody;
	private TriggerComponent Trigger => View.Trigger;
	private AnimationComponent AnimationComponent => View.AnimationComponent;

	public int Id => _startEnemyData.Id;
	public float Speed => _startEnemyData.Speed;
	public Vector2 DirectionMovement => _directionMovement;
	public Transform TransformPosition => View.transform;
	public IObservable<int> HealthStream => _health;

	public PresenterPoolEnemyRobotBase(
		TView view, 
		EnemyData enemyData,
		SignalBus signalBus,
		GameSettings gameSettings) : base(view)
	{
		_signalBus = signalBus;
		_startEnemyData = enemyData;
		_gameSettings = gameSettings;

		Trigger.SetId(enemyData.Id);
	}

	public override void Initialize()
	{
		base.Initialize();

		View.ViewSlider.gameObject.SetActive(true);
		View.ViewSlider.SetStartValue(_startEnemyData.Health, _startEnemyData.Health);

		View.transform.position = _startEnemyData.StartPosition.position;
		Trigger.SetVisible(true);
		ChangeMoveDirection(Vector2.down);
		_health.Value = _startEnemyData.Health;
	}

	public override void Dispose()
	{
		_disposables.Dispose();
		base.Dispose();
	}

	public void DelayedDispose()
	{
		AnimationComponent.Restart();
		View.transform.position = _gameSettings.DefaultPosition;

		Dispose();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position 
			+ _directionMovement * _startEnemyData.Speed * Time.deltaTime);
	}

	public void SetDamage(int damage, out bool isAlive)
	{
		_health.Value -= damage;

		isAlive = _health.Value > 0;

		if (isAlive)
		{
			View.ViewSlider.SetCurrentValue(_health.Value, false);
			return;
		}

		View.ViewSlider.SetCurrentValue(0f);
		View.ViewSlider.gameObject.SetActive(false);
		ChangeMoveDirection(Vector2.zero);
		Trigger.SetVisible(false);

		AnimationComponent.Die();
		View.ParticleSystemDie.Play();
	}

	public void Attack()
	{
		ChangeMoveDirection(Vector2.zero);
		Trigger.SetVisible(false);
		View.ViewSlider.gameObject.SetActive(false);

		AnimationComponent.Attack();
	}

	private void ChangeMoveDirection(Vector2 direction)
	{
		AnimationComponent.Move(direction);
		_directionMovement = direction;
	}
}