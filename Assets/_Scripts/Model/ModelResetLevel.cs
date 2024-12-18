using System;
using UniRx;
using Zenject;

public class ModelResetLevel : IInitializable, IDisposable
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();
	private readonly IResettable[] _resettables;

	public ModelResetLevel(SignalBus signalBus, IResettable[] resettables)
	{
		_signalBus = signalBus;
		_resettables = resettables;
	}

	public void Initialize()
	{
		_signalBus.GetStream<SignalGameNew>().Subscribe(OnGameNew).AddTo(_disposables);
	}

	public void Dispose()
	{
		_disposables.Dispose();		
	}

	private void OnGameNew(SignalGameNew @new)
	{
		foreach (var resettable in _resettables)
			resettable.Reset();
	}
}