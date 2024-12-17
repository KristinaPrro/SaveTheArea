﻿using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PresenterAstronaut : PresenterBase<ViewAstronaut>, ITickable
{
	private readonly IInputModel _inputModel;
	private readonly SignalBus _signalBus;
	private readonly CompositeDisposable _disposables = new();
	private readonly GameSettings _gameSettings;
	private readonly ModelPlayerAttack _modelPlayerAttack;

	private Vector2 _directionMovement;
	private float Speed => _gameSettings.CharacterSpeed;
	private Rigidbody2D Rigidbody => View.Rigidbody;

	public PresenterAstronaut(
		ViewAstronaut view,
		GameSettings gameSettings,
		ModelPlayerAttack modelPlayerAttack,
		SignalBus signalBus,
		IInputModel inputModel)
		: base(view)
	{
		_inputModel = inputModel;
		_signalBus = signalBus;
		_gameSettings = gameSettings;
		_modelPlayerAttack = modelPlayerAttack;
	}

	public override void Initialize()
	{
		_signalBus.GetStream<SignalPlayerFire>().Subscribe(OnFire).AddTo(_disposables);

		_inputModel.DirectionMovementStream.Subscribe(OnDirectionChange).AddTo(_disposables);

		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter).AddTo(_disposables);
		View.Collider.OnTriggerExit2DAsObservable().Subscribe(OnTriggerExit).AddTo(_disposables);

		_modelPlayerAttack.SetContainer(View.ContainerBullet);
	}

	public override void Dispose()
	{
		base.Dispose();

		_disposables.Dispose();
		StopMoving();
	}

	public void Tick()
	{
		Rigidbody.MovePosition(Rigidbody.position + _directionMovement * Speed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:
				if(!other.TryGetComponent<ISpawnElementsView>(out var enemy))
				{
					this.LogError($"{nameof(ISpawnElementsView)} component not found on object with {other.tag} tag!");
					break;
				}

				_modelPlayerAttack.AddTarget(enemy);
				break;
		}
	}

	private void OnTriggerExit(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:
				if(!other.TryGetComponent<ISpawnElementsView>(out var enemy))
				{
					this.LogError($"{nameof(ISpawnElementsView)} component not found on object with {other.tag} tag!");
					break;
				}

				_modelPlayerAttack.RemoveTarget(enemy);
				break;
		}
	}

	private void OnFire(SignalPlayerFire signalData)
	{		
		View.AnimationComponent.Fire();
	}

	public void OnDirectionChange(Vector2 direction)
	{
		if (direction == Vector2.zero)
		{
			StopMoving();
			return;
		}

		View.AnimationComponent.Move(direction);
		_directionMovement = direction;
	}

	private void StopMoving()
	{
		View.AnimationComponent.StopMoving();
		_directionMovement = Vector2.zero;
	}
}