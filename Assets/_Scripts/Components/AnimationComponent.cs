using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	[field: SerializeField]
	public Animator Animator { get; private set; }

	public void Restart()
	{
		SetTrigger(AnimationUtils.ANIM_NAME_START);
	}
			
	public void Move(Vector2 direction)
	{
		Move(direction.x,direction.y);
	}

	public void Move(float x, float y)
	{
		SetFloat(AnimationUtils.ANIM_NAME_MOVE_X, x);
		SetFloat(AnimationUtils.ANIM_NAME_MOVE_Y, y);
		SetTrigger(AnimationUtils.ANIM_NAME_MOVE);
	}

	public void Attack()
	{
		SetTrigger(AnimationUtils.ANIM_NAME_ATTACK);
	}

	public void Hit()
	{
		SetTrigger(AnimationUtils.ANIM_NAME_HIT);
	}

	public void Die()
	{
		SetTrigger(AnimationUtils.ANIM_NAME_DIE);
	}

	private void SetTrigger(string name)
	{
		Animator.SetTrigger(name);
		Animator.SetTrigger(name);
	}
	
	private void SetFloat(string name, float value)
	{
		Animator.SetFloat(name, value);
	}
}