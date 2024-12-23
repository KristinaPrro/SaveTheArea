using System.Linq;
using UnityEngine;

public class ModelPlayerTargetEnemys : ModelObjectBase<IEnemy>
{
	public override void ClearElements() => Presenters.Clear();

	public bool TryGetFirstElementAfterCheckDistanse(Vector2 startPosition, out IEnemy enemy)
	{
		var count = Presenters.Count;
		
		if(Presenters == null || count == 0)
		{
			this.LogWarning($"{nameof(Presenters)} is null!");

			enemy = null;
			return false;
		}

		var sortPresenters = Presenters.OrderBy(e 
			=> Vector2.Distance((Vector2)e.TransformPosition.position, startPosition)).ToList();
		enemy = sortPresenters[0];

		this.LogDebug($"{Presenters.Count}: " +
				$"({Vector2.Distance(sortPresenters[0].TransformPosition.position, startPosition)} ; " +
				$"{Vector2.Distance(sortPresenters[count - 1].TransformPosition.position, startPosition)})");

		return true;
	}
}