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

		this.LogDebug($"! start {startTargetPosition}, {startPosition}, {acceptableError}, " +
			$"{sumSpeed}, {shiftTargetPosition}, {meetPosition}", LogChannel.Todo);
		var i = 0; //todo

		do
		{
			meetPosition = shiftTargetPosition;
			var distance = Vector2.Distance(meetPosition, startTargetPosition)
				+ Vector2.Distance(meetPosition, startPosition);

			var time = distance / sumSpeed;
			shiftTargetPosition = startTargetPosition + targetData.DirectionMovement * targetData.Speed * time;

			this.LogDebug($"! do {meetPosition}, {shiftTargetPosition}, " +
				$"{Vector2.Distance(meetPosition, shiftTargetPosition)} < {acceptableError};     " +
				$"{distance}({Vector2.Distance(meetPosition, startTargetPosition)}+" +
				$"{Vector2.Distance(meetPosition, startPosition)}), {time}, ", LogChannel.Todo);
		}
		while (Vector2.Distance(meetPosition, shiftTargetPosition) > acceptableError && i++<5);

		DirectionMovement = Vector3.ClampMagnitude(shiftTargetPosition - startPosition, 1);
		this.LogDebug($"! finish {DirectionMovement}", LogChannel.Todo);
	}

	public void SetTarget2(TargetData targetData)
	{
		Vector2 startTargetPosition = targetData.Transform.position;
		Vector2 bulletStartPosition = View.transform.position;

		var toTarget = startTargetPosition - bulletStartPosition;
		var targetMovement = targetData.DirectionMovement.normalized * targetData.Speed;

		var a = Vector2.Dot(targetMovement, targetMovement) - Speed * Speed;
		var b = 2 * Vector2.Dot(targetMovement, toTarget);
		var c = Vector2.Dot(toTarget, toTarget);

		var discriminant = b * b - 4 * a * c;

		if (discriminant < 0)
		{
			this.LogError("Target is unattainable");
			return;
		}

		var t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
		var t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

		var timToHit = Mathf.Max(t1, t2);

		var predictedTargetPosition = startTargetPosition + targetMovement * timToHit;

		DirectionMovement = (predictedTargetPosition - bulletStartPosition).normalized;
		this.LogDebug($"! finish {DirectionMovement}", LogChannel.Todo);
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