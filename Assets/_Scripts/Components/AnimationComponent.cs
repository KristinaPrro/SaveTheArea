using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	[field: SerializeField]
	public Animator Animator { get; private set; }

	public void Move(Vector2 direction)
	{
		this.LogDebug($"Move({direction})");
	}

	public void StopMoving()
	{
		this.LogDebug($"StopMoving()");
	}

	public void Fire()
	{
		this.LogDebug($"Fire()");
	}

	public void Hit()
	{
		this.LogDebug($"Hit()");
	}

	public void Die()
	{
		this.LogDebug($"Die()");
	}
}