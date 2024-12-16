using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class PresenterAstronaut : PresenterBase<ViewAstronaut>, ITickable
{
	private readonly IInputModel _inputModel;
	private readonly CompositeDisposable _disposables = new();
	private readonly GameSettings _gameSettings;

	private Vector2 _directionMovement;

	private Rigidbody2D Rigidbody => View.Rigidbody;

	public PresenterAstronaut(
		ViewAstronaut view,
		GameSettings gameSettings,
		IInputModel inputModel)
		: base(view)
	{
		_inputModel = inputModel;
		_gameSettings = gameSettings;

		this.LogDebug($"Move({View?.gameObject?.name})");
	}

	public override void Initialize()
	{
		_inputModel.DirectionMovementStream.Subscribe(Move).AddTo(_disposables);
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * _gameSettings.CharacterSpeed);
	}

	public override void Dispose()
	{
		base.Dispose();

		_disposables.Dispose();
		StopMoving();
	}

	public void Fire(int damage)
	{
		View.AnimationComponent.Fire();
		//pool Bullet
	}

	public void Move(Vector2 direction)
	{
		if (direction == Vector2.zero)
		{
			StopMoving();
			return;
		}

		View.AnimationComponent.Move(direction);
		_directionMovement = direction;
	}

	public void StopMoving()
	{
		View.AnimationComponent.StopMoving();
		_directionMovement = Vector2.zero;
	}
}