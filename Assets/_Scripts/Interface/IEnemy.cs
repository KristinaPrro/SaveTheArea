using UnityEngine;

public interface IEnemy : ISpawnElements
{
	public Vector2 Position { get;}
	public float Speed { get;}
	public Vector2 DirectionMovement { get; }
	public void SetEnemyData(float speed, int health, int id);
	public void Tick();
	public void OnDirectionChange(Vector2 vector);
	public void SetDamage(int damage);
	public void Explode();
}
