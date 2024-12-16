using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class EnemySpawnModel : IInitializable, IDisposable, ITickable
{
	private readonly CompositeDisposable _disposables = new(); 
	private readonly List<Transform> _containerEnemySpawn;
	private readonly GameSettings _gameSettings;
	private readonly PresenterPoolRobotGray.Factory _presenterPoolRobotFactoryGray;
	private readonly List<IEnemy> _presenterEnemys = new();

	private DateTime _nextSpawnTime;
	private int _countEnemy;
	private int _necessaryCountEnemy;
	private float _middleSpeed;

	public EnemySpawnModel(
		List<Transform> containerEnemySpawn,
		PresenterPoolRobotGray.Factory presenterPoolRobotFactoryGray,
		GameSettings gameSettings)
	{
		_containerEnemySpawn = containerEnemySpawn;
		_gameSettings = gameSettings;
		_presenterPoolRobotFactoryGray = presenterPoolRobotFactoryGray;

		this.LogDebug($"EnemySpawnModel");
	}

	public void Initialize()
	{
		Reset();
	}

	public void Dispose()
	{
		_disposables.Dispose();
		_presenterEnemys.Clear();
	}

	public void Tick()
	{
		SpawnEnemy();

		foreach (var enemy in _presenterEnemys)
			enemy.Tick();
	}

	public void SpawnEnemy()
	{
		if (_nextSpawnTime > DateTime.Now || _countEnemy >= _necessaryCountEnemy)
			return;

		this.LogDebug($"Enemy tic");

		var speed = UnityEngine.Random.Range(_gameSettings.EnemySpeedMin, _gameSettings.EnemySpeedMax);
		var maxHealth = _gameSettings.EnemyHealth;
		int spawnPoint = UnityEngine.Random.Range(0, _containerEnemySpawn.Count);

		var presenterEnemy = CreateRobot(speed, spawnPoint).AddTo(_disposables);
		presenterEnemy.SetEnemyData(speed, maxHealth);
		_presenterEnemys.Add(presenterEnemy);

		var timeOut = UnityEngine.Random.Range(_gameSettings.EnemyTimeOutMin, _gameSettings.EnemyTimeOutMax);

		_countEnemy ++;
		_nextSpawnTime = DateTime.Now.AddSeconds(timeOut);
	}

	private IEnemy CreateRobot(float speed, int spawnPoint)
	{
		return _presenterPoolRobotFactoryGray.Create(_containerEnemySpawn[spawnPoint]);
	}

	private void Reset()
	{
		_necessaryCountEnemy = UnityEngine.Random.Range(_gameSettings.EnemyCountMin, _gameSettings.EnemyCountMax);
		_nextSpawnTime = DateTime.MinValue;
		_countEnemy = 0;
	}
}