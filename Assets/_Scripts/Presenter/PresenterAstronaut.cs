using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PresenterAstronaut : PresenterBase<ViewAstronaut>, ITickable
{
	private readonly IInputModel _inputModel;
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();
	private readonly GameSettings _gameSettings;
	private readonly ModelLevel _modelLevel;
	private readonly ModelPlayerAttack _modelPlayerAttack;

	private Vector2 _directionMovement;
	private bool IsOutGame => _modelLevel.IsOutGame;
	private float Speed => _gameSettings.CharacterSpeed;
	private Rigidbody2D Rigidbody => View.Rigidbody;

	public PresenterAstronaut(
		ViewAstronaut view,
		GameSettings gameSettings,
		ModelPlayerAttack modelPlayerAttack,
		ModelLevel modelLevel,
		SignalBus signalBus,
		IInputModel inputModel)
		: base(view)
	{
		_inputModel = inputModel;
		_signalBus = signalBus;
		_gameSettings = gameSettings;
		_modelLevel = modelLevel;
		_modelPlayerAttack = modelPlayerAttack;
	}

	public override void Initialize()
	{
		base.Initialize();

		_signalBus.GetStream<SignalGameNew>().Subscribe(OnGameNew).AddTo(_disposables);
		_signalBus.GetStream<SignalPlayerFire>().Subscribe(OnFire).AddTo(_disposables);

		_modelLevel.CurrentPlayerHealthStream.AsObservable().Subscribe(OnPlayerHealthChange).AddTo(_disposables);
		_modelLevel.OutGameStream.AsObservable().Subscribe(OnGameStateChange).AddTo(_disposables);

		_inputModel.DirectionMovementStream.Subscribe(OnChangeDirection).AddTo(_disposables);

		View.CircleCollider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter).AddTo(_disposables);
		View.CircleCollider.OnTriggerExit2DAsObservable().Subscribe(OnTriggerExit).AddTo(_disposables);

		View.CircleCollider.radius = _gameSettings.CharacterRadiusFire;
		_modelPlayerAttack.SetContainer(View.ContainerBullet);
	}

	public override void Dispose()
	{
		_disposables.Dispose();

		base.Dispose();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * Speed * Time.deltaTime);
	}

	private void OnGameNew(SignalGameNew @new)
	{
		View.Rigidbody.transform.position = View.StartPosition;
	}

	private void OnTriggerEnter(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:

				if (!other.TryGetTriggerId(out int id))
					break;

				_modelPlayerAttack.AddTarget(id);
				break;
		}
	}

	private void OnTriggerExit(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:

				if (!other.TryGetTriggerId(out int id))
					break;

				_modelPlayerAttack.RemoveTarget(id);
				break;
		}
	}

	private void OnGameStateChange(GameState gameState)
	{
		if (IsOutGame)
		{
			ChangeDirection(Vector2.zero);
			return;
		}

		View.AnimationComponent.Restart();
	}

	private void OnFire(SignalPlayerFire signalData)
	{		
		View.AnimationComponent.Attack();
	}

	private void OnPlayerHealthChange(int health)
	{
		if (IsOutGame || health == _modelLevel.MaxPlayerHealt)
			return;
		
		if (health <= 0)
			View.AnimationComponent.Die();
		else
			View.AnimationComponent.Hit();
	}

	private void OnChangeDirection(Vector2 direction)
	{
		if (IsOutGame)
			return;

		ChangeDirection(direction);
	}

	private void ChangeDirection(Vector2 direction)
	{
		View.AnimationComponent.Move(direction);
		_directionMovement = Vector2.ClampMagnitude(direction, 1);
	}
}