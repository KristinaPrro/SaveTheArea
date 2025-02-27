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

	public void SetTarget(TargetData targetData)//todo
	{
		Vector2 startTargetPosition = targetData.Transform.position;
		Vector2 startPosition = View.transform.position;
		var acceptableError = targetData.Transform.lossyScale.y / 2;
		var sumSpeed = Speed + targetData.Speed;//todo

		var shiftTargetPosition = startTargetPosition;
		var meetPosition = startTargetPosition;

		this.LogDebug($"! start {startTargetPosition}, {startPosition}, {acceptableError}, {sumSpeed}, {shiftTargetPosition}, {meetPosition}");
		var i = 0; //todo

		do
		{
			meetPosition = shiftTargetPosition;
			var distance = Vector2.Distance(meetPosition, startTargetPosition)
				+ Vector2.Distance(meetPosition, startPosition);

			var time = distance / sumSpeed;
			shiftTargetPosition = startTargetPosition + targetData.DirectionMovement * targetData.Speed * time;

			this.LogDebug($"! do {meetPosition}, {shiftTargetPosition}, {Vector2.Distance(meetPosition, shiftTargetPosition)} < {acceptableError};     " +
				$"{distance}({Vector2.Distance(meetPosition, startTargetPosition)}+{Vector2.Distance(meetPosition, startPosition)}), {time}, ");
		}
		while (Vector2.Distance(meetPosition, shiftTargetPosition) > acceptableError && i++<5);

		DirectionMovement = Vector3.ClampMagnitude(shiftTargetPosition - startPosition, 1);
		this.LogDebug($"! finish {DirectionMovement}");
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