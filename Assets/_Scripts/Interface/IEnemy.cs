using System;
using UnityEngine;

public interface IEnemy : IDisposable
{
	public void SetEnemyData(float speed, int health);
	public void Tick();
	public void OnDirectionChange(Vector2 vector);
	public void SetDamage(int damage);
	public void Explode();
}
