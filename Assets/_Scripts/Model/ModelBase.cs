using System;
using UniRx;
using Zenject;

public abstract class ModelBase: IInitializable, IDisposable, IResettable
{
	protected readonly CompositeDisposable Disposables = new();
	protected readonly ModelLevel ModelLevel;

	protected bool OutGame => ModelLevel.IsOutGame;

	public ModelBase(ModelLevel modelLevel)
	{
		ModelLevel = modelLevel;
	}

	public virtual void Initialize()
	{
		Reset();
	}

	public virtual void Dispose()
	{
		Disposables.Dispose();
	}

	public virtual void Reset()
	{

	}
}