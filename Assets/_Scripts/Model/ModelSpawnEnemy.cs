using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelSpawnEnemy : ModelBase, ITickable
{
	private readonly List<Transform> _containerEnemySpawn;
	private readonly GameSettings _gameSettings;
	private readonly ModelEnemyObjects _modelEnemyObjects;
	private readonly PresenterPoolEnemyRobotGray.Factory _presenterPoolRobotFactoryGray;

	private DateTime _nextSpawnTime;
	private int _countEnemy;
	private float _middleSpeed;

	public ModelSpawnEnemy(List<Transform> containerEnemySpawn,
		ModelEnemyObjects modelEnemyObjects,
		ModelLevel modelLevel,
		PresenterPoolEnemyRobotGray.Factory presenterPoolRobotFactoryGray, 
		GameSettings gameSettings) 
		: base(modelLevel)
	{
		_containerEnemySpawn = containerEnemySpawn;
		_modelEnemyObjects = modelEnemyObjects;
		_gameSettings = gameSettings;
		_presenterPoolRobotFactoryGray = presenterPoolRobotFactoryGray;
	}

	public override void Initialize()
	{
		base.Initialize();

		Reset();
	}

	public void Tick()
	{
		if (OutGame)
			return;

		SpawnEnemy();
	}

	public override void Reset()
	{
		base.Reset();

		_nextSpawnTime = DateTime.MinValue;
		_countEnemy = 0;
		_modelEnemyObjects.Reset();
	}

	public void SpawnEnemy()
	{
		if (_nextSpawnTime > DateTime.Now || _countEnemy >= ModelLevel.MaxEnemyCount)
			return;

		this.LogDebug($"Enemy spawn", LogChannel.SpawnObject);

		var speed = UnityEngine.Random.Range(_gameSettings.EnemySpeedMin, _gameSettings.EnemySpeedMax);
		var maxHealth = _gameSettings.EnemyHealth;
		int spawnPoint = UnityEngine.Random.Range(0, _containerEnemySpawn.Count);

		var presenterEnemy = CreateRobot(spawnPoint, maxHealth, speed).AddTo(Disposables);
		_modelEnemyObjects.AddEnemy(presenterEnemy);

		var timeOut = UnityEngine.Random.Range(_gameSettings.EnemyTimeOutMin, _gameSettings.EnemyTimeOutMax);

		_countEnemy ++;
		_nextSpawnTime = DateTime.Now.AddSeconds(timeOut);
	}

	private IEnemy CreateRobot(int spawnPoint, int maxHealth, float speed)
	{
		return _presenterPoolRobotFactoryGray.Create(_containerEnemySpawn[spawnPoint], 
			new EnemyData(SpawnIdUtils.GetNext(), _containerEnemySpawn[spawnPoint], maxHealth, speed));
	}
}