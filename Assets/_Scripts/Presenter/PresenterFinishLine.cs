using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PresenterFinishLine: PresenterBase<ViewFinishLine>
{
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();

	public PresenterFinishLine(
		SignalBus signalBus,
		ViewFinishLine view) : base(view)
	{
		_signalBus = signalBus;
	}

	public override void Initialize()
	{
		base.Initialize();
		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter).AddTo(_disposables);
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	private void OnTriggerEnter(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:
				if (!other.TryGetComponent<TriggerComponent>(out var trigger))
				{
					this.LogError($"{nameof(TriggerComponent)} component not found on object with {other.tag} tag!");
					break;
				}

				_signalBus.Fire(new SignalEnemyReachedFinish(trigger.Id));
				break;
		}
	}
}