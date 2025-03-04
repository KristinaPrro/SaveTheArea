using System;
using Zenject;

public abstract class Presenter : IInitializable, IDisposable
{
	public virtual void Initialize()
	{

	}

	public virtual void Dispose()
	{

	}
}