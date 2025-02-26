using UnityEngine;
using Zenject;

public class PresenterPoolEnemyRobotGray : PresenterPoolEnemyRobotBase<ViewPoolEnemyRobotGray>
{
	public PresenterPoolEnemyRobotGray(
		ViewPoolEnemyRobotGray view, 
		EnemyData enemyData, 
		SignalBus signalBus, 
		GameSettings gameSettings)
		: base(view, enemyData, signalBus, gameSettings)
	{
	}

	public class Factory : PlaceholderFactory<Transform, EnemyData, PresenterPoolEnemyRobotGray>
	{
	}
}