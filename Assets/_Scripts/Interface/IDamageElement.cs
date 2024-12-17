using System;
using UnityEngine;

public interface IDamageElement : ISpawnElements
{
	public void SetData(float speed, int damage, int id);
	public void Tick();
	public void SetTarget(Vector2 targetPosition,
		float speed,
		Vector2 targetDirectionMovement);
}