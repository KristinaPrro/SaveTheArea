using System;
using UnityEngine;

public interface IInputModel
{
	IObservable<Vector2> DirectionMovementStream { get; }
	Vector2 DirectionMovement { get; }
}