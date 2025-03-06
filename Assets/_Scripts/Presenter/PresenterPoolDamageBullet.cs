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
		Vector2 startBulletPosition = View.transform.position;
		var acceptableError = targetData.Transform.lossyScale.y / 2;
		var sumSpeed = Speed + targetData.Speed;

		var shiftTargetPosition = startTargetPosition;
		var meetPosition = startTargetPosition;
		var countAttemps = 0;

		do
		{
			meetPosition = shiftTargetPosition;
			var distance = Vector2.Distance(meetPosition, startTargetPosition)
				+ Vector2.Distance(meetPosition, startBulletPosition);

			var time = distance / sumSpeed;
			shiftTargetPosition = startTargetPosition + targetData.DirectionMovement * targetData.Speed * time;

			this.LogDebug($"TARGET_CALCULATE({countAttemps}) meetPosition{meetPosition}; {shiftTargetPosition}; " +
				$"startBulletPosition:{startBulletPosition}; startTargetPosition:{startTargetPosition}", 
				LogChannel.Attack);
		}
		while (Vector2.Distance(meetPosition, shiftTargetPosition) > acceptableError
			&& countAttemps++ < Utils.MAX_COUNT_ATTEMPS_TARGET_CALCULATE);

		DirectionMovement = (shiftTargetPosition - startBulletPosition).normalized;
		this.LogDebug($"DirectionMovement:{DirectionMovement};", LogChannel.Attack);
	}

	public class Factory : PlaceholderFactory<Transform, DamageElementData, PresenterPoolDamageBullet>
	{
		public PresenterPoolDamageBullet Create(Transform param, DamageElementData data, TargetData targetData)
		{
			var presenter = base.Create(param, data);
			presenter.SetTarget(targetData);
			return presenter;
		}
	}
}