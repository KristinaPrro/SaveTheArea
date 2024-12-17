using System;
using System.Collections.Generic;
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
	private int _necessaryCountEnemy;
	private float _middleSpeed;

	public ModelSpawnEnemy(List<Transform> containerEnemySpawn,
		ModelEnemyObjects modelEnemyObjects,
		PresenterPoolEnemyRobotGray.Factory presenterPoolRobotFactoryGray, 
		GameSettings gameSettings) 
		: base()
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
		SpawnEnemy();
	}

	public void SpawnEnemy()
	{
		if (_nextSpawnTime > DateTime.Now || _countEnemy >= _necessaryCountEnemy)
			return;

		this.Log($"Enemy spown");

		var speed = UnityEngine.Random.Range(_gameSettings.EnemySpeedMin, _gameSettings.EnemySpeedMax);
		var maxHealth = _gameSettings.EnemyHealth;
		int spawnPoint = UnityEngine.Random.Range(0, _containerEnemySpawn.Count);

		var presenterEnemy = CreateRobot(speed, spawnPoint);
		presenterEnemy.SetEnemyData(speed, maxHealth, IdHandler.GetNext());
		_modelEnemyObjects.AddElement(presenterEnemy);

		var timeOut = UnityEngine.Random.Range(_gameSettings.EnemyTimeOutMin, _gameSettings.EnemyTimeOutMax);

		_countEnemy ++;
		_nextSpawnTime = DateTime.Now.AddSeconds(timeOut);
	}

	public override void Reset()
	{
		base.Reset();

		_necessaryCountEnemy = UnityEngine.Random.Range(_gameSettings.EnemyCountMin, _gameSettings.EnemyCountMax);
		_nextSpawnTime = DateTime.MinValue;
		_countEnemy = 0;
	}

	private IEnemy CreateRobot(float speed, int spawnPoint)
	{
		return _presenterPoolRobotFactoryGray.Create(_containerEnemySpawn[spawnPoint]);
	}
}