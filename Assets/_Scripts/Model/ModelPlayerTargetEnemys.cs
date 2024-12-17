using System.Linq;
using UnityEngine;
using Zenject;

public class ModelPlayerTargetEnemys : ModelObjectBase<IEnemy>, ITickable
{
	public void Tick()
	{
		foreach (var enemy in Presenters)
			enemy.Tick();
	}

	public bool TryGetFirstElementAfterCheckDistanse(Vector2 startPosition, out IEnemy enemy)
	{
		if(Presenters == null || Presenters.Count == 0)
		{
			this.LogError($"{nameof(Presenters)} is null!");

			enemy = null;
			return false;
		}

		Presenters.OrderBy(e => Vector2.Distance(e.Position, startPosition));
		enemy = Presenters[0];

		return true;
	}
}