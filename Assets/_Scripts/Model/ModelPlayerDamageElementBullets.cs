using UniRx;
using Zenject;

public class ModelPlayerDamageElementBullets : ModelBase, ITickable
{
	private readonly SignalBus _signalBus;
	private readonly ModelPlayerDamageElements _modelPlayerDamageElements;

	public ModelPlayerDamageElementBullets(
		SignalBus signalBus,
		GameSettings gameSettings,
		ModelPlayerDamageElements modelPlayerDamageElements) : base()
	{
		_signalBus = signalBus;
		_modelPlayerDamageElements = modelPlayerDamageElements;
	}

	public override void Initialize()
	{
		base.Initialize();

		_signalBus.GetStream<SignalDisappearanceDamageElement>().Subscribe(OnDisappearance).AddTo(Disposables);
		_signalBus.GetStream<SignalEnemyDamage>().Subscribe(OnEnemyDamage).AddTo(Disposables);
		Reset();
	}


	public void Tick()
	{
		foreach (var enemy in _modelPlayerDamageElements.Presenters)
			enemy.Tick();
	}


	private void OnDisappearance(SignalDisappearanceDamageElement signalData)
	{
		DisposeById(signalData.DamageElementId);
	}

	private void OnEnemyDamage(SignalEnemyDamage signalData)
	{
		DisposeById(signalData.DamageElementId);
	}

	private void DisposeById(int id)
	{
		_modelPlayerDamageElements.DisposeElementById(id);
	}
}