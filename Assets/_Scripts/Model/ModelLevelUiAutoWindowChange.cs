using UniRx;
using Zenject;

public class ModelLevelUiAutoWindowChange: ModelBase
{
	private readonly SignalBus _signalBus;
	private readonly ModelLevelUi _modelLevelUi;

	public ModelLevelUiAutoWindowChange(ModelLevel modelLevel, SignalBus signalBus, ModelLevelUi modelLevelUi)
		: base(modelLevel)
	{
		_signalBus = signalBus;
		_modelLevelUi = modelLevelUi;
	}

	public override void Initialize()
	{
		base.Initialize();

		_signalBus.GetStream<SignalGameNew>().Subscribe(OnGameNew).AddTo(Disposables);
		_signalBus.GetStream<SignalGameResults>().Subscribe(OnGameResults).AddTo(Disposables);

		_modelLevelUi.CloseAll();
		_modelLevelUi.Open(WindowType.GameStatusScreencreen);
	}

	private void OnGameNew(SignalGameNew signalData)
	{
		this.LogDebug($"OnGameNew()");
		_modelLevelUi.ChangeScreen(WindowType.GameStatusScreencreen);		
	}

	private void OnGameResults(SignalGameResults signalData)
	{
		_modelLevelUi.ChangeScreen(WindowType.ResultScreen);
	}
}