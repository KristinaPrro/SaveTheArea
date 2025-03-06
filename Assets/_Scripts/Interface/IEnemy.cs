using UnityEngine;

public interface IEnemy : ISpawnElements
{
	public Transform TransformPosition { get;}
	public float Speed { get;}
	public bool IsUnderGun { get;}
	public Vector2 DirectionMovement { get; }
	public void FixedTick();
	public void SetDamage(int damage, out bool isAlive);
	public void DelayedDispose();
	public void Attack();
	public void SetTargetState(bool isUnderGun);
}