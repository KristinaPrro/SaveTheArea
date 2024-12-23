using UnityEngine;

public struct EnemyData
{
	public int Id { get; private set; }
	public Transform StartPosition { get; private set; }
	public int Health { get; private set; }
	public float Speed { get; private set; }

	public EnemyData(int id, Transform startPosition, int health, float speed)
	{
		Id = id;
		StartPosition = startPosition;
		Health = health;
		Speed = speed;
	}
}