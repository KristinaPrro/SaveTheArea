using UniRx;
using Zenject;

public class PresenterLevelParticle : PresenterBase<ViewLevelParticle>
{
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;
	private readonly ModelLevel _modelLevel;

	public PresenterLevelParticle(
		SignalBus signalBus,
		ModelLevel modelLevel,
		ViewLevelParticle view) 
		: base(view)
	{
		_signalBus = signalBus;
		_modelLevel = modelLevel;
	}

	public override void Initialize()
	{
		_modelLevel.OutGameStream.AsObservable().Subscribe(OnGameStateChange).AddTo(_disposables);

		base.Initialize();
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	private void OnGameStateChange(GameState state)
	{
		if (state != GameState.Win)
			return;

		View.ParticleSystemWin.Play();	
	}
}