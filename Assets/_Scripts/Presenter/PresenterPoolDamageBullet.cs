using UnityEngine;
using Zenject;

public class PresenterPoolDamageBullet : PresenterPoolDamageElement<ViewPoolDamageBullet>
{
	public PresenterPoolDamageBullet(
		ViewPoolDamageBullet view, 
		DamageElementData damageElementData, 
		SignalBus signalBus) 
		: base(view, damageElementData, signalBus)
	{
	}

	public void SetTarget(TargetData targetData)
	{
		Vector2 startTargetPosition = targetData.Transform.position;
		var startPosition = View.transform.position;
		var acceptableError = targetData.Transform.lossyScale.y / 2;
		var sumSpeed = Speed + targetData.Speed;

		var shiftTargetPosition = startTargetPosition;
		var meetPosition = startTargetPosition;

		do
		{
			meetPosition = shiftTargetPosition;
			var distance = Vector2.Distance(meetPosition, startTargetPosition)
				+ Vector2.Distance(meetPosition, startPosition);

			var time = distance / sumSpeed;
			shiftTargetPosition = startTargetPosition + targetData.DirectionMovement * targetData.Speed * time;
		}
		while (Vector2.Distance(meetPosition, shiftTargetPosition) < acceptableError);

		DirectionMovement = Vector3.ClampMagnitude(shiftTargetPosition - (Vector2)startPosition, 1);
	}

	public class Factory : PlaceholderFactory<Transform, DamageElementData, PresenterPoolDamageBullet>
	{
		public PresenterPoolDamageBullet Create(Transform param, DamageElementData data, TargetData targetData)
		{
			 var presenter = base.Create(param, data);

			presenter.Initialize();
			presenter.SetTarget(targetData);

			return presenter;
		}
	}
}