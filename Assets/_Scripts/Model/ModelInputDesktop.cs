﻿using System;
using UniRx;
using UnityEngine;
using Zenject;

public class ModelInputDesktop : IInputModel, ITickable
{
	private readonly ReactiveProperty<Vector2> _directionMovementProperty = new();

	public IObservable<Vector2> DirectionMovementStream => _directionMovementProperty;
	public Vector2 DirectionMovement => _directionMovementProperty.Value;

	public ModelInputDesktop()
	{
	}

	public void Tick()
	{
		_directionMovementProperty.Value = GetDirection();
	}

	private Vector2 GetDirection()
	{
		var direction = Vector2.zero;

		if (!Input.anyKey)
			return direction;

		if (Input.GetKey(KeyCode.W))
			direction += Vector2.up;

		if (Input.GetKey(KeyCode.S))
			direction += Vector2.down;

		if (Input.GetKey(KeyCode.D))
			direction += Vector2.right;

		if (Input.GetKey(KeyCode.A))
			direction += Vector2.left;

		return direction;
	}
}