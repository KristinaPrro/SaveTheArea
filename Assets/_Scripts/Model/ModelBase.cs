using System;
using UniRx;
using Zenject;

public abstract class ModelBase: IInitializable, IDisposable, IReset
{
	protected readonly CompositeDisposable Disposables = new();

	public ModelBase()
	{
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