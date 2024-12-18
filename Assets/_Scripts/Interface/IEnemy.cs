﻿using UnityEngine;

public interface IEnemy : ISpawnElements
{
	public Transform TransformPosition { get;}
	public float Speed { get;}
	public int Health { get;}
	public Vector2 DirectionMovement { get; }
	public void SetEnemyData(float speed, int health, int id);
	public void Tick();
	public void OnDirectionChange(Vector2 vector);
	public void SetDamage(int damage);
	public void Explode();
}
