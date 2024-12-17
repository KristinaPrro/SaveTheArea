using System;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelSpawnDamageElement : ModelSpawnBase<IDamageElement>, ITickable
{
	private readonly CompositeDisposable _disposables = new(); 
	private readonly GameSettings _gameSettings;
	private readonly PresenterPoolDamageBullet.Factory _presenterPoolDamageBulletFactory;

	private Transform _containerSpawnDamageElement;
	private DateTime _nextSpawnTime;

	protected float RateFire => _gameSettings.CharacterRateFire;

	public ModelSpawnDamageElement(
		Transform containerEnemySpawn,
		PresenterPoolDamageBullet.Factory presenterPoolDamageBulletFactory,
		GameSettings gameSettings)
	{
		_containerSpawnDamageElement = containerEnemySpawn;
		_gameSettings = gameSettings;
		_presenterPoolDamageBulletFactory = presenterPoolDamageBulletFactory;

		this.LogDebug($" DamageElementSpawnModel");
	}

	public override void Initialize()
	{
		base.Initialize();
	}

	public void Tick()
	{
		foreach (var enemy in Presenters)
			enemy.Tick();
	}

	public void SpawnDamageElement(Transform containerSpawnDamageElement)
	{
		if (_nextSpawnTime > DateTime.Now)
			return;

		this.LogDebug($"Enemy spawn");

		var presenterDamageElement = CreateDamageElement(containerSpawnDamageElement).AddTo(_disposables);
		
		Presenters.Add(presenterDamageElement);

		_nextSpawnTime = DateTime.Now.AddSeconds(RateFire);
	}

	private IDamageElement CreateDamageElement(Transform containerEnemySpawn)
	{
		var presenter = _presenterPoolDamageBulletFactory.Create(containerEnemySpawn);

		presenter.SetData(
			_gameSettings.CharacterBulletSpeed, 
			_gameSettings.CharacterDamagePerShot, 
			IdHandler.GetNext());

		return presenter;
	}

	protected override void Reset()
	{
		//_necessaryCountEnemy = UnityEngine.Random.Range(_gameSettings.EnemyCountMin, _gameSettings.EnemyCountMax);
		//_nextSpawnTime = DateTime.MinValue;
		//_countEnemy = 0;
	}
}