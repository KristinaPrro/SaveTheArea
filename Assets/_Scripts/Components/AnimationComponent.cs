using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
	private const string ANIM_NAME_START = "START";
	private const string ANIM_NAME_MOVE = "Move";
	private const string ANIM_NAME_MOVE_X = "MoveX";
	private const string ANIM_NAME_MOVE_Y = "MoveY";
	private const string ANIM_NAME_ATTACK = "Attack";
	private const string ANIM_NAME_HIT = "Hit";
	private const string ANIM_NAME_DIE = "Die";

	private const float ANIM_MOVE_IDLE = 0;
	
	[field: SerializeField]
	public Animator Animator { get; private set; }

	public void Restart()
	{
		SetTrigger(ANIM_NAME_START);
	}
			
	public void Move(Vector2 direction)
	{
		Move(direction.x,direction.y);
	}

	public void Move(float x, float y)
	{
		SetFloat(ANIM_NAME_MOVE_X, x);
		SetFloat(ANIM_NAME_MOVE_Y, y);
		SetTrigger(ANIM_NAME_MOVE);
	}

	public void StopMoving()
	{
		Move(ANIM_MOVE_IDLE, ANIM_MOVE_IDLE);
	}

	public void Fire()
	{
		SetTrigger(ANIM_NAME_ATTACK);
	}

	public void Hit()
	{
		SetTrigger(ANIM_NAME_HIT);
	}

	public void Die()
	{
		SetTrigger(ANIM_NAME_DIE);
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