using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ModelPlayerTargetEnemys : IDisposable, IResettable
{
	protected readonly List<IEnemy> _presenters = new();
	public virtual void Dispose()
	{
		Reset();
	}

	public virtual void Reset()
	{
		_presenters.Clear();
	}

	public void AddTarget(IEnemy element)
	{
		element.SetTargetState(true);
		
		_presenters.Add(element);
		this.LogDebug($": {element.Id} ({_presenters.Count}:  {Debug()})", LogChannel.Attack); 
	}

	public void RemoveTarget(IEnemy element)
	{
		if (!_presenters.Contains(element))
		{
			this.LogError($"Target not found! ({_presenters.Count}:  {Debug()})");
			return;
		}

		element.SetTargetState(false);
		_presenters.Remove(element);
		this.LogDebug($": {element.Id} ({_presenters.Count}:  {Debug()})", LogChannel.SpawnObject);
	}

	public void RemoveTarget(int id)
	{
		if (!TryGetElementById(id, out var element))
			return;

		element.SetTargetState(false);
		_presenters.Remove(element);
		this.LogDebug($": {element.Id} ({_presenters.Count}:  {Debug()})", LogChannel.SpawnObject); 
	}

	public bool TryGetFirstElementAfterCheckDistanse(Vector2 startPosition, out IEnemy enemy)
	{
		var count = _presenters.Count;

		if (_presenters == null || count == 0)
		{
			this.LogWarning($"{nameof(_presenters)} is null!");

			enemy = null;
			return false;
		}

		var sortPresenters = _presenters
			.OrderBy(e => Vector2.Distance((Vector2)e.TransformPosition.position, startPosition))
			.ToList();

		enemy = sortPresenters[0];

		this.LogDebug($"{_presenters.Count}: " +
			$"({Vector2.Distance(sortPresenters[0].TransformPosition.position, startPosition)} ; " +
			$"{Vector2.Distance(sortPresenters[count - 1].TransformPosition.position, startPosition)})" +
			$"         {Debug()}; ", LogChannel.Attack);

		return true;
	}

	private bool TryGetElementById(int id, out IEnemy element)
	{
		element = _presenters.Find(e => e.Id == id);

		if (element == null)
		{
			this.LogError($"Object by Id {id} not found! ({_presenters.Count}:  {Debug()})");
			return false;
		}

		return true;
	}

	private string Debug()
	{
		string s = "";

		foreach (var p in _presenters)
			s = $"{s} {p.Id.ToString()};";

		return s;
	}
}