using UnityEngine;
using Zenject;

public class PresenterPoolRobotGray : PresenterPoolRobotBase<ViewPoolRobotGray>
{
	public PresenterPoolRobotGray(ViewPoolRobotGray view, SignalBus signalBus) : base(view, signalBus)
	{
	}

	public class Factory : PlaceholderFactory<Transform, PresenterPoolRobotGray>
	{
	}
}