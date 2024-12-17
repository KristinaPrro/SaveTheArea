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
			this.LogError($"{nameof(Presenters)} is null!");

			enemy = null;
			return false;
		}

		Presenters.OrderBy(e => Vector2.Distance(e.Position, startPosition));
		enemy = Presenters[0];

		this.LogDebug($"{Presenters.Count}: ({Vector2.Distance(Presenters[0].Position, startPosition)} ; " +
			$"{Vector2.Distance(Presenters[count - 1].Position, startPosition)})");

		return true;
	}
}