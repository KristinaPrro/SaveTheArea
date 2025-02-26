using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

public class ModelEnemy : ModelBase, ITickable
{
	private readonly SignalBus _signalBus;
	private readonly ModelEnemyObjects _modelEnemyObjects;
	private readonly GameSettings _gameSettings;
	private readonly ModelPlayerTargetEnemys _modelPlayerTargetEnemys;

	public ModelEnemy(
		ModelLevel modelLevel,
		SignalBus signalBus,
		GameSettings gameSettings,
		ModelPlayerTargetEnemys modelPlayerTargetEnemys,
		ModelEnemyObjects modelPlayerDamageElements) : base(modelLevel)
	{
		_signalBus = signalBus;
		_gameSettings = gameSettings;
		_modelEnemyObjects = modelPlayerDamageElements;
		_modelPlayerTargetEnemys = modelPlayerTargetEnemys;
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
		if (!_modelEnemyObjects.TryGetElementById(signalData.EnemyId, out var enemy))
			return;

		enemy.Attack();

		EnemyDie(enemy).Forget();
	}

	private void OnEnemyDamage(SignalEnemyDamage signalData)
	{
		this.LogDebug($"{signalData.EnemyId}");

		if (!_modelEnemyObjects.TryGetElementById(signalData.EnemyId, out var enemy))
			return;

		enemy.SetDamage(signalData.Damage, out bool isAlive);

		if (isAlive)
		{
			this.Log($" EnemyId:{signalData.EnemyId}; isAlive:{isAlive};");
			return;
		}

		_modelPlayerTargetEnemys.RemoveElement(enemy);

		_signalBus.Fire(new SignalEnemyDie(signalData.EnemyId));

		EnemyDie(enemy).Forget();
	}

	private async UniTaskVoid EnemyDie(IEnemy enemy)
	{
		try
		{
			await UniTask.Delay(AnimationUtils.DELAYED_DISTROY_ROBOT_TIME);
		}
		finally
		{
			enemy?.DelayedDispose();
			_modelEnemyObjects?.RemoveElement(enemy);
		}
	}
}