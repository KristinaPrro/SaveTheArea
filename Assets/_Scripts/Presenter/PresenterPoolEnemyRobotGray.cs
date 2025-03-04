using UnityEngine;
using Zenject;

public class PresenterPoolEnemyRobotGray : PresenterPoolEnemyRobotBase<ViewPoolEnemyRobotGray>
{
	public PresenterPoolEnemyRobotGray(
		ViewPoolEnemyRobotGray view, 
		EnemyData enemyData, 
		SignalBus signalBus, 
		GameSettings gameSettings,
		IInstantiator instantiator)
		: base(view, enemyData, signalBus, gameSettings, instantiator)
	{
	}

	public class Factory : PlaceholderFactory<Transform, EnemyData, PresenterPoolEnemyRobotGray>
	{
	}
}