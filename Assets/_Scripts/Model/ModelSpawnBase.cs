using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

public abstract class ModelSpawnBase<T> : IInitializable, IDisposable
	where T:ISpawnElements
{
	protected readonly CompositeDisposable Disposables = new();
	protected readonly List<T> Presenters = new();

	public ModelSpawnBase()
	{
		this.LogDebug($"SpawnModel");
	}

	public virtual void Initialize()
	{
	}

	public virtual void Dispose()
	{
		DisposeSpownElements(Presenters);
		Presenters.Clear();
		
		Disposables.Dispose();
	}

	protected virtual void Reset()
	{

	}

	protected void DestroyElementById(int id, List<ISpawnElements> presenters)
	{
		var element = presenters.Find(e => e.Id == id);

		if (element == null)
		{
			this.LogError($"DamageElement by Id {id} is null!");
			return;
		}

		presenters.Remove(element);
		element.Dispose();
	}

	protected void DisposeSpownElements(IList<T> presenters)
	{
		while (presenters.Count > 0)
		{
			var idx = presenters.Count - 1;
			var element = presenters[idx];
			presenters.RemoveAt(idx);
			element.Dispose();
		}
	}
}