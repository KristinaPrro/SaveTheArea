using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class ModelObjectBase<T> : IDisposable, IReset where T : ISpawnElements
{
	protected readonly List<T> Presenters = new();

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
	public virtual bool TryGetElementById(int id, out T element) => TryGetElementById(id, Presenters, out element);
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
		if(!TryGetElementById(id, Presenters, out var element))
			return;

		Presenters.Remove(element);
		element.Dispose();
	}

	protected bool TryGetElementById(int id, IList<T> presenters, out T element)
	{		
		var i = presenters.Count -1;
		while ( i >= 0)
		{
			element = presenters[i];
			if (element.Id == id)
			{
				return true;
			}
		}

		element = default(T);
		this.LogError($"{nameof(T)} by Id {id} not found!");
		return false;
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