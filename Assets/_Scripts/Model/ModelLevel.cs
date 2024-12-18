using System;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelLevel: IInitializable, IDisposable, IResettable
{
	private readonly SignalBus _signalBus;
	private readonly GameSettings _gameSettings;
	private readonly CompositeDisposable _disposables = new();

	private ReactiveProperty<int> _currentPlayerHealth = new();
	private ReactiveProperty<int> _currentEnemyCount = new();
	private ReactiveProperty<int> _maxPlayerHealth = new();
	private ReactiveProperty<int> _maxEnemyCount = new();

	public IObservable<int> CurrentPlayerHealthStream => _currentPlayerHealth;
	public IObservable<int> CurrentEnemyCountStream => _currentEnemyCount;
	public int CurrentPlayerHealt => _currentPlayerHealth.Value;
	public int CurrentEnemyCount => _currentEnemyCount.Value;
	public int MaxPlayerHealt => _maxPlayerHealth.Value;
	public int MaxEnemyCount => _maxEnemyCount.Value;
	public bool OutGame { get; private set; }

	public ModelLevel(SignalBus signalBus, GameSettings gameSettings)
	{
		_signalBus = signalBus;
		_gameSettings = gameSettings;
	}

	public void Initialize()
	{
		Reset();

		_signalBus.GetStream<SignalEnemyReachedFinish>().Subscribe(OnEnemyReachedFinish).AddTo(_disposables);
		_signalBus.GetStream<SignalEnemyDie>().Subscribe(OnEnemyDie).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	public void Reset()
	{
		var health = _gameSettings.CharacterHealth;
		_currentPlayerHealth.Value = health;
		_maxPlayerHealth.Value = health;

		var enemyCount = UnityEngine.Random.Range(_gameSettings.EnemyCountMin, _gameSettings.EnemyCountMax);
		_currentEnemyCount.Value = enemyCount;
		_maxEnemyCount.Value = enemyCount;

		OutGame = false;
	}

	public void Exit()
	{
		Application.Quit();
	}

	private void OnEnemyReachedFinish(SignalEnemyReachedFinish signalData)
	{
		_currentPlayerHealth.Value--;
		_currentEnemyCount.Value--;

		if (_currentPlayerHealth.Value > 0)
			return;

		FinishGame(false);
	}
	
	private void OnEnemyDie(SignalEnemyDie signalData)
	{
		_currentEnemyCount.Value--;

		if (_currentEnemyCount.Value > 0)
			return;

		FinishGame(true);
	}

	private void FinishGame(bool isWin)
	{
		OutGame = true;
		_signalBus.Fire(new SignalGameResults(isWin,
			new GameResultsData(
				_currentPlayerHealth.Value, _maxPlayerHealth.Value,
				_currentEnemyCount.Value, _maxEnemyCount.Value)));
	}
}