using System;
using UnityEngine;

public interface IEnemy : ISpawnElements
{
	public void SetEnemyData(float speed, int health, int id);
	public void Tick();
	public void OnDirectionChange(Vector2 vector);
	public void SetDamage(int damage);
	public void Explode();
}