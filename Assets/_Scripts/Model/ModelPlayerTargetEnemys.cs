using System.Linq;
using UnityEngine;

public class ModelPlayerTargetEnemys : ModelObjectBase<IEnemy>
{
	protected override void ClearElements() => Presenters.Clear();
	public void AddTarget(IEnemy element)
	{
		AddElement(element);
		element.SetTargetState(true);
	}

	public void RemoveTarget(IEnemy element)
	{
		element.SetTargetState(false);
		RemoveElement(element);
	}

	public void RemoveTarget(int id)
	{
		if (!TryGetElementById(id, out var element))
			return;

		element.SetTargetState(false);
		Presenters.Remove(element);
		this.LogDebug($": {element.Id} ({Presenters.Count}:  {Debug()})", LogChannel.SpawnObject); 
	}

	public bool TryGetFirstElementAfterCheckDistanse(Vector2 startPosition, out IEnemy enemy)
	{
		var count = Presenters.Count;

		if (Presenters == null || count == 0)
		{
			this.LogWarning($"{nameof(Presenters)} is null!");

			enemy = null;
			return false;
		}

		var sortPresenters = Presenters
			.OrderBy(e => Vector2.Distance((Vector2)e.TransformPosition.position, startPosition))
			.ToList();

		enemy = sortPresenters[0];

		this.LogDebug($"{Presenters.Count}: " +
			$"({Vector2.Distance(sortPresenters[0].TransformPosition.position, startPosition)} ; " +
			$"{Vector2.Distance(sortPresenters[count - 1].TransformPosition.position, startPosition)})" +
			$"         {Debug()}; ", LogChannel.Attack);

		return true;
	}
}