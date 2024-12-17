using UnityEngine;
using Zenject;

public class PresenterPoolDamageBullet : PresenterPoolDamageElement<ViewPoolDamageBullet>
{
	public PresenterPoolDamageBullet(ViewPoolDamageBullet view, SignalBus signalBus) : base(view, signalBus)
	{
	}

	public class Factory : PlaceholderFactory<Transform, PresenterPoolDamageBullet>
	{
	}
}