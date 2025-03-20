using UniRx;
using Zenject;

public class UiPresenterLobbyMain : UiPresenterScreenBase<UiViewLobbyMain>
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();

	public override WindowType WindowType => WindowType.Main;

	public UiPresenterLobbyMain(
		UiViewLobbyMain view,
		SignalBus signalBus,
		ModelUiScreenChange sceneUiModel) : base(view, sceneUiModel)
	{
		_signalBus= signalBus;
	}

	public override void Initialize()
	{
		base.Initialize();

		View.ButtonStart.OnClickAsObservable().Subscribe(OnClickButtonStart).AddTo(_disposables);
		View.ButtonSettings.OnClickAsObservable().Subscribe(OnClickButtonSettings).AddTo(_disposables);
		View.ButtonCustomize.OnClickAsObservable().Subscribe(OnClickButtonCustomize).AddTo(_disposables);
		View.ButtonThanks.OnClickAsObservable().Subscribe(OnClickButtonThanks).AddTo(_disposables);
		View.ButtonExit.OnClickAsObservable().Subscribe(OnClickButtonExit).AddTo(_disposables);
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	private void OnClickButtonExit(Unit _)
	{
		this.LogDebug("", LogChannel.Todo);
	}

	private void OnClickButtonThanks(Unit _)
	{
		this.LogDebug("", LogChannel.Todo);
	}

	private void OnClickButtonCustomize(Unit _)
	{
		this.LogDebug("", LogChannel.Todo);
	}

	private void OnClickButtonSettings(Unit _)
	{
		this.LogDebug("", LogChannel.Todo);
	}

	private void OnClickButtonStart(Unit _)
	{
		_signalBus.Fire(new SignalCoreChangeScene(SceneType.SaveArea));
	}
}