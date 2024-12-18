using System;
using System.Collections.Generic;

public class ModelObjectBase<T> : IDisposable, IResettable where T : ISpawnElements
{
	private readonly List<T> _presenters = new();

	public List<T> Presenters => _presenters;

	public virtual void Dispose()
	{
		Reset();
	}

	public virtual void Reset()
	{
		ClearElements();
	}

	public virtual void AddElement(T element) => Presenters.Add(element);
	public virtual void ClearElements() => DisposeSpownElements(Presenters);
	public virtual void RemoveElement(T enemy)
	{
		if(!Presenters.Contains(enemy))
		{
			this.LogError($"{nameof(T)} not found!");
			return;
		}

		Presenters.Remove(enemy);
	}

	public virtual void RemoveElementById(int id)
	{
		if(!TryGetElementById(id, out var element))
			return;

		Presenters.Remove(element);
	}

	public virtual void DisposeElementById(int id)
	{
		if(!TryGetElementById(id, out var element))
			return;

		Presenters.Remove(element);
		element.Dispose();
	}

	public virtual bool TryGetElementById(int id, out T element)
	{
		element = Presenters.Find(e => e.Id == id);

		if (element == null)
		{
			this.LogError($"DamageElement by Id {id} is null!");
			return false;
		}

		return true;
	}

	protected void DisposeSpownElements(IList<T> presenters)
	{
		while (presenters.Count > 0)
		{
			var idx = presenters.Count - 1;
			var element = presenters[idx];
			element.Dispose();
			presenters.RemoveAt(idx);
		}
	}
}