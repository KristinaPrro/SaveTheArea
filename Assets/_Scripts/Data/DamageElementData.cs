using UnityEngine;

public struct DamageElementData
{
	public int Id { get; private set; }
	public int Damage { get; private set; }
	public float Speed { get; private set; }
	public Transform StartPosition { get; private set; }

	public DamageElementData(int id, int damage, float speed, Transform startPosition)
	{
		Id = id;
		Damage = damage;
		Speed = speed;
		StartPosition = startPosition;
	}
}