using System;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelPlayerAttack : ModelBase, ITickable
{
	private readonly SignalBus _signalBus; 
	private readonly GameSettings _gameSettings;
	private readonly ModelPlayerTargetEnemys _modelPlayerTargetEnemys;
	private readonly ModelEnemyObjects _modelEnemyObjects;
	private readonly ModelPlayerSpawnDamageElement _modelPlayerSpawnDamageElement;

	private readonly ReactiveProperty<bool> _isAttackTimeProperty = new();

	private Transform _containerSpawn;
	private DateTime _nextAttackTime;

	protected float RateFire => _gameSettings.CharacterRateFire;

	public ModelPlayerAttack(
		SignalBus signalBus,
		GameSettings gameSettings,
		ModelEnemyObjects modelEnemyObjects,
		ModelPlayerTargetEnemys modelPlayerTargetEnemys,
		ModelPlayerSpawnDamageElement modelPlayerSpawnDamageElement) : base()
	{
		_signalBus = signalBus;
		_gameSettings = gameSettings;
		_modelPlayerSpawnDamageElement = modelPlayerSpawnDamageElement;
		_modelPlayerTargetEnemys = modelPlayerTargetEnemys;
		_modelEnemyObjects = modelEnemyObjects;
	}

	public override void Initialize()
	{
		base.Initialize();

		_isAttackTimeProperty.SkipLatestValueOnSubscribe().Subscribe(OnCheckTarget).AddTo(Disposables);

		Reset();
	}

	public void Tick()
	{
		_isAttackTimeProperty.Value = _nextAttackTime < DateTime.Now;
	}

	public void SetContainer(Transform containerSpawnDamageElement)
	{
		_containerSpawn = containerSpawnDamageElement;
	}

	public void AddTarget(int id)
	{
		if (!_modelEnemyObjects.TryGetElementById(id, out var element))
			return;

		_modelPlayerTargetEnemys.AddElement(element);
		TryFire();
	}

	public void RemoveTarget(ITriggerComponent enemy)
	{
		_modelPlayerTargetEnemys.RemoveElementById(enemy.Id);
	}

	private void OnCheckTarget(bool isSpawnTime)
	{
		TryFire();
	}

	public void TryFire()
	{
		if (!_isAttackTimeProperty.Value)
			return;

		if (!_modelPlayerTargetEnemys.TryGetFirstElementAfterCheckDistanse(_containerSpawn.position, out var enemy))
			return;

		SpawnDamageElementWithTarget(_containerSpawn, enemy.Position, enemy.Speed, enemy.DirectionMovement);

		_signalBus.Fire(new SignalPlayerFire());
	}

	public void SpawnDamageElementWithTarget(Transform containerSpawnDamageElement,
		Vector2 targetPosition,
		float speed,
		Vector2 targetDirectionMovement)
	{
		var presenter = _modelPlayerSpawnDamageElement.CreateDamageElement(containerSpawnDamageElement).AddTo(Disposables);
		presenter.SetTarget(targetPosition, speed, targetDirectionMovement);

		_nextAttackTime = DateTime.Now.AddSeconds(RateFire);
	}

	
	public override void Reset()
	{
		base.Reset();

		_nextAttackTime = DateTime.MinValue;
	}
}