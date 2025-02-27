﻿using System;
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

	public virtual void AddElement(T element)
	{
		_presenters.Add(element);
		this.LogDebug($": {element.Id} ({_presenters.Count})", LogChannel.SpawnObject);
	}

	public virtual void ClearElements() => DisposeSpownElements(_presenters);
	public virtual void RemoveElement(T element)
	{
		if(!_presenters.Contains(element))
		{
			this.LogError($"{nameof(T)} not found! ({_presenters.Count})");
			return;
		}

		_presenters.Remove(element);
		this.LogDebug($": {element.Id} ({_presenters.Count})", LogChannel.SpawnObject);
	}

	public virtual void RemoveElementById(int id)
	{
		if(!TryGetElementById(id, out var element))
			return;

		_presenters.Remove(element);
		this.LogDebug($": {element.Id} ({_presenters.Count})", LogChannel.SpawnObject);
	}

	public virtual void DisposeElementById(int id)
	{
		if(!TryGetElementById(id, out var element))
			return;

		_presenters.Remove(element);
		element.Dispose();

		this.LogDebug($": {element.Id} ({_presenters.Count})", LogChannel.SpawnObject);
	}

	public virtual bool TryGetElementById(int id, out T element)
	{
		element = _presenters.Find(e => e.Id == id);

		if (element == null)
		{
			this.LogError($"Object by Id {id} not found! ({_presenters.Count})");
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