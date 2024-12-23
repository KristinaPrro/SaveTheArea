using UnityEngine;
using Zenject;

public class PresenterPoolEnemyRobotGray : PresenterPoolEnemyRobotBase<ViewPoolEnemyRobotGray>
{
	public PresenterPoolEnemyRobotGray(ViewPoolEnemyRobotGray view, EnemyData enemyData, SignalBus signalBus)
		: base(view, enemyData, signalBus)
	{
	}

	public class Factory : PlaceholderFactory<Transform, EnemyData, PresenterPoolEnemyRobotGray>
	{
	}
}