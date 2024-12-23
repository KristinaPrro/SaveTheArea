using UnityEngine;

public struct TargetData
{
	public Transform Transform { get; private set; }
	public float Speed { get; private set; }
	public Vector2 DirectionMovement { get; private set; }

	public TargetData(Transform transform, float speed, Vector2 directionMovement)
	{
		Transform = transform;
		Speed = speed;
		DirectionMovement = directionMovement;
	}
}