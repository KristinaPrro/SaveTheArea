using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	[field: SerializeField]
	public Animator Animator { get; private set; }

	public void Move(Vector2 direction)
	{
		this.Log($"Move({direction})");
	}

	public void StopMoving()
	{
		this.Log($"StopMoving()");
	}

	public void Fire()
	{
		this.Log($"Fire()");
	}

	public void Hit()
	{
		this.Log($"Hit()");
	}

	public void Die()
	{
		this.Log($"Die()");
	}
}