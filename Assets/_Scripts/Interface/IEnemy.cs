using UnityEngine;

public interface IEnemy
{
	public void SetEnemyData(float speed, int health);
	public void Move(Vector2 vector);
	public void SetDamage(int damage);
	public void Explode();
}
