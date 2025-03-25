using Cysharp.Threading.Tasks;
using System;
using UniRx;
using Zenject;

public class ModelStartScene : IInitializable, IDisposable
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();

	public ModelStartScene(SignalBus signalBus)
	{
		_signalBus = signalBus;
	}

	public void Initialize()
	{
		StartGame().Forget();
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private async UniTaskVoid StartGame()
	{
		await UniTask.Delay(SceneUtils.START_LOADING_SCENE_DELAY_MLS);

		_signalBus.Fire(new SignalCoreChangeScene(SceneType.Lobby));
	}
}