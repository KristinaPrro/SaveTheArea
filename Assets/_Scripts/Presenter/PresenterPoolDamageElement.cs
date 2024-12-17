﻿using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public abstract class PresenterPoolDamageElement<TView> : PresenterPoolBase<TView>, ITickable, IDamageElement
	 where TView : ViewPoolDamageElement
{
	private readonly CompositeDisposable _disposables = new();
	private readonly SignalBus _signalBus;

	private Vector2 _directionMovement;

	protected int Damage { get; set; }
	protected float Speed { get; set; }
	protected Rigidbody2D Rigidbody => View.Rigidbody;

	public int Id { get; private set; }

	public PresenterPoolDamageElement(TView view, SignalBus signalBus) : base(view)
	{
		_signalBus = signalBus;
	}

	public override void Initialize()
	{
		View.Collider.OnTriggerEnter2DAsObservable().Subscribe(OnTriggerEnter2D).AddTo(_disposables);
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

	public void SetData(float speed, int damage, int id)
	{
		Speed = speed;
		Damage = damage;
		Id = id;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case ObjectUtils.ROBOT_TAG:
				if (!other.TryGetComponent<ISpawnElementsView>(out var enemy))
				{
					this.LogError($"{nameof(ISpawnElementsView)} component not found on object with {other.tag} tag!");
					break;
				}

				_signalBus.Fire(new SignalPlayerDamage( Id, enemy.Id)); //todo
				break;
		}
	}

	public void SetTarget(Vector2 targetPosition,
	float speed,
	Vector2 targetDirectionMovement)
	{
		//var time = 1;//todo
		//var d = (targetPosition + targetDirectionMovement*speed*time -View.transform.position)/speed;
		//var direction = new Vector2 ((targetPosition));
		var direction = (targetPosition - targetDirectionMovement) 
			/ Vector2.Distance(targetPosition, targetDirectionMovement);

		_directionMovement = direction;
	}

	public void StopMoving()
	{
		_directionMovement = Vector2.zero;
	}

	public void Disappear() //todo
	{

	}
}