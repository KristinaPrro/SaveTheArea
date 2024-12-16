using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawnModel : IInitializable, IDisposable, ITickable
{
	private readonly List<Transform> _containerEnemySpawn;
	private readonly GameSettings _gameSettings;
	private readonly PresenterPoolRobotGray.Factory _presenterPoolRobotFactoryGray;
	//private readonly PresenterPoolRobotRed.Factory _presenterPoolRobotFactoryRed;
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

	}

	public void Dispose()
	{

	}

	public void Tick()
	{
		if (_nextSpawnTime > DateTime.Now || _countEnemy >= _necessaryCountEnemy)
			return;

		SpawnEnemy();

		var timeOut = UnityEngine.Random.Range(_gameSettings.EnemyTimeOutMin, _gameSettings.EnemyTimeOutMax);

		_countEnemy ++;
		_nextSpawnTime = DateTime.Now.AddSeconds(timeOut);
	}

	public void SpawnEnemy()
	{
		var speed = UnityEngine.Random.Range(_gameSettings.EnemySpeedMin, _gameSettings.EnemySpeedMax);
		var maxHealth = _gameSettings.EnemyHealth;
		int spawnPoint = UnityEngine.Random.Range(0, _containerEnemySpawn.Count-1);

		var presenterEnemy = CreateRobot(speed, spawnPoint);
		presenterEnemy.SetEnemyData(speed, maxHealth);
		_presenterEnemys.Add(presenterEnemy);
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