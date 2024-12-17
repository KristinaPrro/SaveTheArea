using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelSpawnEnemy : ModelSpawnBase<IEnemy>, ITickable
{
	private readonly List<Transform> _containerEnemySpawn;
	private readonly GameSettings _gameSettings;
	private readonly PresenterPoolEnemyRobotGray.Factory _presenterPoolRobotFactoryGray;

	private DateTime _nextSpawnTime;
	private int _countEnemy;
	private int _necessaryCountEnemy;
	private float _middleSpeed;

	public ModelSpawnEnemy(List<Transform> containerEnemySpawn, 
		PresenterPoolEnemyRobotGray.Factory presenterPoolRobotFactoryGray, 
		GameSettings gameSettings) 
		: base()
	{
	}

	public override void Initialize()
	{
		base.Initialize();

		Reset();
	}

	public void Tick()
	{
		SpawnEnemy();

		foreach (var enemy in Presenters)
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

		var presenterEnemy = CreateRobot(speed, spawnPoint).AddTo(Disposables);
		presenterEnemy.SetEnemyData(speed, maxHealth, IdHandler.GetNext());
		Presenters.Add(presenterEnemy);

		var timeOut = UnityEngine.Random.Range(_gameSettings.EnemyTimeOutMin, _gameSettings.EnemyTimeOutMax);

		_countEnemy ++;
		_nextSpawnTime = DateTime.Now.AddSeconds(timeOut);
	}

	protected override void Reset()
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