using UniRx;
using Zenject;

public class UiPresenterGameStatusScreen : UiPresenterScreenBase<UiViewGameStatusScreen>
{
	private readonly CompositeDisposable _disposables = new();
	private readonly ModelLevel _modelLevel;
	private readonly SignalBus _signalBus;

	public override WindowType WindowType => WindowType.GameStatusScreen;

	public UiPresenterGameStatusScreen(
		UiViewGameStatusScreen view,
		ModelLevel modelLevel,
		SignalBus signalBus,
		ModelUiScreenChange sceneUiModel) 
		: base(view, sceneUiModel)
	{
		_modelLevel = modelLevel;
		_signalBus = signalBus;
	}

	public override void Initialize()
	{
		base.Initialize();

		_modelLevel.CurrentPlayerHealthStream.AsObservable().Subscribe(OnPlayerHealthChange).AddTo(_disposables);
		_modelLevel.CurrentEnemyCountStream.AsObservable().Subscribe(OnEnemyCountChange).AddTo(_disposables);

		View.ButtonExit.OnClickAsObservable().Subscribe(OnExit).AddTo(_disposables);
	}

	private void OnExit(Unit _)
	{
		_signalBus.Fire(new SignalCoreChangeScene(SceneType.Lobby));
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	private void OnEnemyCountChange(int health)
	{
		View.TextEnemyCount.text = health.ToString();
	}

	private void OnPlayerHealthChange(int enemyCount)
	{
		View.TextPlayerHealth.text = enemyCount.ToString();
	}
}