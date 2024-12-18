using UnityEngine;

public interface IDamageElement : ISpawnElements
{
	public void SetData(float speed, int damage, int id);
	public void Tick();
	public void SetTarget(Transform targetPosition,
		float speed,
		Vector2 targetDirectionMovement, 
		Transform startPosition);
}