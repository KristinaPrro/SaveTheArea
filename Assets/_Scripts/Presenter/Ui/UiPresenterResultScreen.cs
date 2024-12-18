using UniRx;
using Zenject;

public class UiPresenterResultScreen : UiPresenterScreenBase<UiViewResultScreen>
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();

	private GameResultsData _gameResultsData;
	
	public override WindowType WindowType => WindowType.ResultScreen;

	public UiPresenterResultScreen(
		UiViewResultScreen view,
		SignalBus signalBus,
		ModelLevelUi sceneUiModel) : base(view, sceneUiModel)
	{
		_signalBus= signalBus;
	}

	public override void Initialize()
	{
		base.Initialize();

		View.ButtonNewGame.OnClickAsObservable().Subscribe(OnClickButtonNewGame).AddTo(_disposables);

		_signalBus.GetStream<SignalGameResults>().Subscribe(OnGameResults).AddTo(_disposables);
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	private void OnGameResults(SignalGameResults signalData)
	{
		View.PanelDefeat.SetActive(!signalData.IsWin);
		View.PanelWin.SetActive(signalData.IsWin);

		var gameResultsData = signalData.GameResultsData;
		_gameResultsData = gameResultsData;
		View.TextDebug.text = string.Format("Health: {0}/{1}; EnemyCount: {2}/{3};",
			gameResultsData.CurrentPlayerHealth, gameResultsData.MaxPlayerHealth,
			gameResultsData.CurrentEnemyCount, gameResultsData.MaxEnemyCount);
	}

	private void OnClickButtonNewGame(Unit unit)
	{
		_signalBus.Fire(new SignalGameNew());
	}
}