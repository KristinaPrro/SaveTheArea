using UnityEngine;

public interface IEnemy : ISpawnElements
{
	public Transform TransformPosition { get;}
	public float Speed { get;}
	public int Health { get;}
	public Vector2 DirectionMovement { get; }
	public void Tick();
	public void SetDamage(int damage);
	public void Attack();
	public void Die();
}
