using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	[field: SerializeField]
	public Animator Animator { get; private set; }

	public void Move(Vector2 direction)
	{
		this.LogDebug($"Move({direction})", LogChannel.Animation);
	}

	public void StopMoving()
	{
		this.LogDebug($"StopMoving()", LogChannel.Animation);
	}

	public void Fire()
	{
		this.LogDebug($"Fire()", LogChannel.Animation);
	}

	public void Hit()
	{
		this.LogDebug($"Hit()", LogChannel.Animation);
	}

	public void Die()
	{
		this.LogDebug($"Die()", LogChannel.Animation);
	}
}