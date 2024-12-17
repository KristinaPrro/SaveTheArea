using UnityEngine;
using Zenject;

public class PresenterPoolEnemyRobotGray : PresenterPoolEnemyRobotBase<ViewPoolEnemyRobotGray>
{
	public PresenterPoolEnemyRobotGray(ViewPoolEnemyRobotGray view, SignalBus signalBus) : base(view, signalBus)
	{
	}

	public class Factory : PlaceholderFactory<Transform, PresenterPoolEnemyRobotGray>
	{
	}
}