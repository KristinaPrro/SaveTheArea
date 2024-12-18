using UniRx;
using Zenject;

public class ModelEnemy : ModelBase, ITickable
{
	private readonly SignalBus _signalBus;
	private readonly ModelEnemyObjects _modelEnemyObjects;
	private readonly GameSettings _gameSettings;

	public ModelEnemy(
		ModelLevel modelLevel,
		SignalBus signalBus,
		GameSettings gameSettings,
		ModelEnemyObjects modelPlayerDamageElements) : base(modelLevel)
	{
		_signalBus = signalBus;
		_gameSettings = gameSettings;
		_modelEnemyObjects = modelPlayerDamageElements;
	}

	public override void Initialize()
	{
		base.Initialize();

		_signalBus.GetStream<SignalEnemyReachedFinish>().Subscribe(OnEnemyReachedFinish).AddTo(Disposables);
		_signalBus.GetStream<SignalEnemyDamage>().Subscribe(OnEnemyDamage).AddTo(Disposables);
		Reset();
	}

	public void Tick()
	{
		if (OutGame)
			return;

		foreach (var enemy in _modelEnemyObjects.Presenters)
			enemy.Tick();
	}

	private void OnEnemyReachedFinish(SignalEnemyReachedFinish signalData)
	{
		DisposeById(signalData.EnemyId);
	}

	private void OnEnemyDamage(SignalEnemyDamage signalData)
	{
		if (!_modelEnemyObjects.TryGetElementById(signalData.EnemyId, out var enemy))
			return;

		enemy.SetDamage(signalData.Damage);

		if (enemy.Health <= 0)
		{
			_signalBus.Fire(new SignalEnemyDie(signalData.EnemyId));
			DisposeById(signalData.EnemyId);
		}
	}

	private void DisposeById(int id)
	{
		_modelEnemyObjects.DisposeElementById(id);
	}
}