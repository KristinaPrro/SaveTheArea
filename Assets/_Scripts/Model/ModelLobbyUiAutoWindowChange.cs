using System;
using UniRx;
using Zenject;

public class ModelLobbyUiAutoWindowChange: IInitializable, IDisposable
{
	protected readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;
	private readonly ModelUiScreenChange _modelLevelUi;

	public ModelLobbyUiAutoWindowChange(SignalBus signalBus, ModelUiScreenChange modelLevelUi)
	{
		_signalBus = signalBus;
		_modelLevelUi = modelLevelUi;
	}

	public void Initialize()
	{
		ChangeScreen(WindowType.Main);
	}

	public void Dispose()
	{
		_disposables.Dispose();
	}

	private void ChangeScreen(WindowType type)
	{
		_modelLevelUi.ChangeScreen(type);
	}
}