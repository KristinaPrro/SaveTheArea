using UniRx;
using Zenject;

public class ModelLevelUiAutoWindowChange: ModelBase
{
	private readonly SignalBus _signalBus;
	private readonly ModelUiScreenChange _modelLevelUi;

	public ModelLevelUiAutoWindowChange(ModelLevel modelLevel, SignalBus signalBus, ModelUiScreenChange modelLevelUi)
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
		_modelLevelUi.Open(WindowType.GameStatusScreen);
	}

	private void OnGameNew(SignalGameNew signalData)
	{
		this.Log($"");
		_modelLevelUi.ChangeScreen(WindowType.GameStatusScreen);		
	}

	private void OnGameResults(SignalGameResults signalData)
	{
		_modelLevelUi.ChangeScreen(WindowType.ResultScreen);
	}
}