using UniRx;

public class UiPresenterGameStatusScreen : UiPresenterScreenBase<UiViewGameStatusScreen>
{
	private readonly CompositeDisposable _disposables = new();
	private readonly ModelLevel _modelLevel;

	public override LevelWindowType WindowType => LevelWindowType.GameStatusScreencreen;

	public UiPresenterGameStatusScreen(
		UiViewGameStatusScreen view,
		ModelLevel modelLevel,
		ModelLevelUi sceneUiModel) 
		: base(view, sceneUiModel)
	{
		_modelLevel = modelLevel;
	}

	public override void Initialize()
	{
		base.Initialize();

		_modelLevel.CurrentPlayerHealthStream.AsObservable().Subscribe(OnPlayerHealthChange).AddTo(_disposables);
		_modelLevel.CurrentEnemyCountStream.AsObservable().Subscribe(OnEnemyCountChange).AddTo(_disposables);

		View.ButtonExit.OnClickAsObservable().Subscribe(OnExit).AddTo(_disposables);
	}

	private void OnExit(Unit unit)
	{
		_modelLevel.Exit();
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